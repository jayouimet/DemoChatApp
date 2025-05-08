using ChatApp.Common.Dtos.Message;
using ChatApp.Common.Responses.Base;
using ChatApp.Server.Entities;
using ChatApp.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageController : BaseController<IMessageService>
{
    public MessageController(ILogger logger, IConfiguration configuration, IMessageService service) : base(logger, configuration, service)
    {
    }

    [Authorize]
    [HttpGet("messages")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiSuccessResponse<List<MessageDto>>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
    public async Task<IActionResult> GetMessages([FromQuery] long userId, CancellationToken cancellationToken)
    {
        List<MessageDto> result = await _service.GetMessages(userId, cancellationToken);
        return new OkObjectResult(result);
    }
}