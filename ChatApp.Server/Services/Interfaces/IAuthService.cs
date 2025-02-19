using ChatApp.Common.Dtos.Auth;
using ChatApp.Common.Responses.Auth;

namespace ChatApp.Server.Services.Interfaces;

public interface IAuthService : IBaseService
{
    public Task<AuthResponse> Login(LoginDto dto, CancellationToken cancellationToken);
    public Task<AuthResponse> Register(RegisterDto dto, CancellationToken cancellationToken);
    public Task<AuthResponse> Refresh(RefreshDto dto, CancellationToken cancellationToken);
}