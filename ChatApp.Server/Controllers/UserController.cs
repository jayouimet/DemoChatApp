using ChatApp.Common.Responses.Base;
using ChatApp.Common.Dtos.User;
using ChatApp.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : BaseController<IUserService>
{
    public UserController(ILogger logger, IConfiguration configuration, IUserService service) : base(logger, configuration, service)
    {
    }

    [Authorize]
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiSuccessResponse<List<UserDto>>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
    public async Task<IActionResult> GetContacts(CancellationToken cancellationToken)
    {
        List<UserDto> result = await _service.GetContacts(cancellationToken);
        return new OkObjectResult(result);
    }
}