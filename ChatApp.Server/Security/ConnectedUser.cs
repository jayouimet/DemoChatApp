using System.Security.Claims;
using ChatApp.Server.Security.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ChatApp.Server.Security;

public class ConnectedUser(IHttpContextAccessor httpContextAccessor) : IConnectedUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    
    public int? Id {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var claim = user?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);
            if (claim is null) return null;
            return int.Parse(claim.Value);
        }
    }
}