using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ChatApp.Common.Responses.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace ChatApp.Client.Providers;

public class AuthStateProvider : AuthenticationStateProvider
{
    private const string AccessTokenKey = "access_token";
    private const string RefreshTokenKey = "refresh_token";
    
    private readonly IJSRuntime _jsRuntime;

    public AuthStateProvider(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<ConnectedUser?> GetConnectedUserAsync()
    {
        var accessToken = await GetAccessToken();
        
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return null;
        }
            
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(accessToken);
            
        int id = int.Parse(jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sid).Value);
        string email = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value;
        string userName = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value;

        return new ConnectedUser()
        {
            Id = id,
            Email = email,
            UserName = userName
        };
    }

    public async Task<string?> GetAccessToken()
    {
        return await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", AccessTokenKey);
    }
    
    public async Task<string?> GetRefreshToken()
    {
        return await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", RefreshTokenKey);
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var accessToken = await GetAccessToken();
        
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(accessToken);

        var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public async Task Login(AuthResponse data)
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", AccessTokenKey, data.AccessToken);
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", RefreshTokenKey, data.RefreshToken);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(data.AccessToken);
        var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task Logout()
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", AccessTokenKey);
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", RefreshTokenKey);

        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }
}