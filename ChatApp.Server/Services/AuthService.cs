using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using ChatApp.Common;
using ChatApp.Common.Dtos.Auth;
using ChatApp.Common.Errors;
using ChatApp.Common.Responses.Auth;
using ChatApp.Server.Entities;
using ChatApp.Server.Entities.Context;
using ChatApp.Server.Services.Abstracts;
using ChatApp.Server.Services.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ChatApp.Server.Services;

public class AuthService : BaseService, IAuthService
{
    public AuthService(
        IConfiguration configuration, 
        ILogger logger, 
        ChatAppContext context, 
        IMapper mapper) : 
        base(configuration, logger, context, mapper)
    {
    }

    public async Task<AuthResponse> Register(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        if (await _context.Users.AsNoTracking().AnyAsync(x => x.UserName == registerDto.UserName, cancellationToken: cancellationToken))
        {
            throw new BadRequestError(ErrorKeys.UsernameAlreadyInUse, "Username is already in use");
        }
        
        if (await _context.Users.AsNoTracking().AnyAsync(x => x.Email == registerDto.Email, cancellationToken: cancellationToken))
        {
            throw new BadRequestError(ErrorKeys.EmailAlreadyInUse, "Email is already in use");
        }
        
        var user = _mapper.Map<User>(registerDto);
        user.PasswordHash = HashPassword(registerDto.Password);
        
        string refreshToken = GenerateRefreshToken();
        DateTime expiresAt = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Auth:RefreshTokenExpirationMinutes"));
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = expiresAt;
        
        var entity = await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        var newUser = entity.Entity;
        
        string accessToken = GenerateJwtToken(newUser);
        
        return new AuthResponse() { RefreshToken = refreshToken, AccessToken = accessToken };
    }

    public async Task<AuthResponse> Login(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email, cancellationToken);
        if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            throw new BadRequestError(ErrorKeys.IncorrectCredentials, "Incorrect credentials");
        }
        
        string accessToken = GenerateJwtToken(user);
        string refreshToken = GenerateRefreshToken();
        DateTime expiresAt = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Auth:RefreshTokenExpirationMinutes"));
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = expiresAt;
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new AuthResponse() { RefreshToken = refreshToken, AccessToken = accessToken };
    }

    public async Task<AuthResponse> Refresh(RefreshDto refreshDto, CancellationToken cancellationToken)
    {
        DateTime now = DateTime.Now;
        
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(refreshDto.AccessToken);

        if (jwtToken == null || !jwtToken.Claims.Any())
        {
            throw new UnauthorizedError(ErrorKeys.JwtInvalid, "Invalid JWT token");
        }
        
        int userId = int.Parse(jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sid).Value);
        
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user == null)
        {
            throw new UnauthorizedError(ErrorKeys.JwtInvalid, "Invalid JWT token");
        }
        
        if (user.RefreshToken != refreshDto.RefreshToken || user.RefreshTokenExpiry < now)
        {
            throw new UnauthorizedError(ErrorKeys.RefreshTokenError, "Invalid refresh token");
        }
        
        string accessToken = GenerateJwtToken(user);
        string refreshToken = GenerateRefreshToken();
        DateTime expiresAt = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Auth:RefreshTokenExpirationMinutes"));
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = expiresAt;
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new AuthResponse() { RefreshToken = refreshToken, AccessToken = accessToken };
    }
    
    #region Privates
    private void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
    {
        buffer[offset] = (byte)(value >> 24);
        buffer[offset + 1] = (byte)(value >> 16);
        buffer[offset + 2] = (byte)(value >> 8);
        buffer[offset + 3] = (byte)(value);
    }
    
    private uint ReadNetworkByteOrder(byte[] buffer, int offset)
    {
        return (uint)buffer[offset] << 24 | 
               (uint)buffer[offset + 1] << 16 | 
               (uint)buffer[offset + 2] << 8 |
               buffer[offset + 3];
    }
    
    private string HashPassword(string password)
    {
        var prf = KeyDerivationPrf.HMACSHA512;
        var rng = RandomNumberGenerator.Create();
        const int iterationCount = 10000;
        const int saltSize = 128 / 8;
        const int numBytesRequested = 256 / 8;
        
        var salt = new byte[saltSize];
        rng.GetBytes(salt);
        var subKey = KeyDerivation.Pbkdf2(password, salt, prf, iterationCount, numBytesRequested);
        
        var outputBytes = new byte[13 + salt.Length + subKey.Length];
        outputBytes[0] = 0x01;
        
        WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
        WriteNetworkByteOrder(outputBytes, 5, iterationCount);
        WriteNetworkByteOrder(outputBytes, 9, saltSize);
        
        Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
        Buffer.BlockCopy(subKey, 0, outputBytes, 13 + saltSize, subKey.Length);
        return Convert.ToBase64String(outputBytes);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        var decodedHashedPassword = Convert.FromBase64String(hashedPassword);

        if (decodedHashedPassword[0] != 0x01)
        {
            return false;
        }
        
        var prf = (KeyDerivationPrf)ReadNetworkByteOrder(decodedHashedPassword, 1);
        var iterCount = (int)ReadNetworkByteOrder(decodedHashedPassword, 5);
        var saltLength = (int)ReadNetworkByteOrder(decodedHashedPassword, 9);

        if (saltLength < 128 / 8)
        {
            return false;
        }
        
        var salt = new byte[saltLength];
        Buffer.BlockCopy(decodedHashedPassword, 13, salt, 0, salt.Length);
        
        var subkeyLength = decodedHashedPassword.Length - 13 - salt.Length;

        if (subkeyLength < 128 / 8)
        {
            return false;
        }
        
        var expectedSubKey = new byte[subkeyLength];
        Buffer.BlockCopy(decodedHashedPassword, 13 + salt.Length, expectedSubKey, 0, expectedSubKey.Length);
        
        var actualSubKey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subkeyLength);
        return actualSubKey.SequenceEqual(expectedSubKey);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateJwtToken(User user)
    {
        var jwtKey = _configuration["Auth:JwtKey"]!;
        var jwtIssuer = _configuration["Auth:JwtIssuer"]!;
        var jwtAudience = _configuration["Auth:JwtAudience"]!;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Auth:JwtExpirationMinutes")),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    #endregion
}