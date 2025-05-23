using AutoMapper;
using ChatApp.Common.Dtos.User;
using ChatApp.Server.Entities.Context;
using ChatApp.Server.Security.Interfaces;
using ChatApp.Server.Services.Abstracts;
using ChatApp.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Server.Services;

public class UserService(
    IConfiguration configuration,
    ILogger logger,
    ChatAppContext context,
    IMapper mapper,
    IConnectedUser connectedUser)
    : BaseService(configuration, logger, context, mapper), IUserService
{
    private readonly IConnectedUser _connectedUser = connectedUser;
    
    public async Task<List<UserDto>> GetContacts(CancellationToken cancellationToken)
    {
        var users = await _context.Users.Where(x => _connectedUser.Id != x.Id).ToListAsync(cancellationToken);
        return _mapper.Map<List<UserDto>>(users);
    }
}