using AutoMapper;
using ChatApp.Common.Dtos.User;
using ChatApp.Server.Entities.Context;
using ChatApp.Server.Security.Interfaces;
using ChatApp.Server.Services.Abstracts;
using ChatApp.Server.Services.Interfaces;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Server.Services;

public class UserService : BaseService, IUserService
{
    private IConnectedUser _connectedUser;
    
    public UserService(
        IConfiguration configuration, 
        ILogger logger, 
        ChatAppContext context, 
        IMapper mapper, 
        IConnectedUser connectedUser) : 
        base(configuration, logger, context, mapper)
    {
        _connectedUser = connectedUser;
    }

    public async Task<List<UserDto>> GetContacts(CancellationToken cancellationToken)
    {
        var users = await _context.Users.Where(x => _connectedUser.Id != x.Id).ToListAsync(cancellationToken);
        return _mapper.Map<List<UserDto>>(users);
    }
}