using ChatApp.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Server.Controllers;

public abstract class BaseController<T> : ControllerBase where T : IBaseService
{
    protected readonly ILogger _logger;
    protected readonly IConfiguration _configuration;
    protected readonly T _service;
    
    public BaseController(ILogger logger, IConfiguration configuration, T service)
    {
        _logger = logger;
        _configuration = configuration;
        _service = service;
    }
}