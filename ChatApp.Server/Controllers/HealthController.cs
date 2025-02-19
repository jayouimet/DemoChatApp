using ChatApp.Common.Responses.Base;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{
	public HealthController()
	{
	}

	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiSuccessResponse<string>))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiErrorResponse))]
	public IActionResult Index()
	{
        return Ok("Healthy");
	}
}