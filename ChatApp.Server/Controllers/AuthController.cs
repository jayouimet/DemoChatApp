using ChatApp.Common.Dtos.Auth;
using ChatApp.Common.Responses.Auth;
using ChatApp.Common.Responses.Base;
using ChatApp.Server.Services;
using ChatApp.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseController<IAuthService>
{
    public AuthController(ILogger logger, IConfiguration configuration, IAuthService service) : base(logger, configuration, service)
    {
    }

    [HttpPost("register")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiSuccessResponse<AuthResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto, CancellationToken cancellationToken)
    {
        AuthResponse authResponse = await _service.Register(dto, cancellationToken);
        return new ObjectResult(authResponse) { StatusCode = StatusCodes.Status201Created };
    }

    [HttpPost("login")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiSuccessResponse<AuthResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]
    public async Task<IActionResult> Register([FromBody] LoginDto dto, CancellationToken cancellationToken)
    {
        AuthResponse authResponse = await _service.Login(dto, cancellationToken);
        return new OkObjectResult(authResponse);
    }

    [HttpPost("refresh")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiSuccessResponse<AuthResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
    public async Task<IActionResult> Register([FromBody] RefreshDto dto, CancellationToken cancellationToken)
    {
        AuthResponse authResponse = await _service.Refresh(dto, cancellationToken);
        return new OkObjectResult(authResponse);
    }
}