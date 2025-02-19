using AutoMapper;
using ChatApp.Server.Entities.Context;
using ChatApp.Server.Services.Interfaces;

namespace ChatApp.Server.Services.Abstracts;

public abstract class BaseService : IBaseService
{
    protected readonly ILogger _logger;
    protected readonly IConfiguration _configuration;
    protected readonly ChatAppContext _context;
    protected readonly IMapper _mapper;
    
    public BaseService(
        IConfiguration configuration, 
        ILogger logger, 
        ChatAppContext context,
        IMapper mapper)
    {
        _configuration = configuration;
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }
}