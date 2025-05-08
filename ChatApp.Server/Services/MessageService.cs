using AutoMapper;
using ChatApp.Common.Dtos.Message;
using ChatApp.Server.Entities;
using ChatApp.Server.Entities.Context;
using ChatApp.Server.Security.Interfaces;
using ChatApp.Server.Services.Abstracts;
using ChatApp.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Server.Services;

public class MessageService(
    IConfiguration configuration,
    ILogger logger,
    ChatAppContext context,
    IMapper mapper,
    IConnectedUser connectedUser)
    : BaseService(configuration, logger, context, mapper), IMessageService
{
    private readonly IConnectedUser _connectedUser = connectedUser;
    public async Task<List<MessageDto>> GetMessages(long userId, CancellationToken cancellationToken)
    {
        var messages = await _context.Messages.Where(Message.IsConcerned(_connectedUser.Id, userId)).ToListAsync(cancellationToken);
        return _mapper.Map<List<MessageDto>>(messages);
    }
}