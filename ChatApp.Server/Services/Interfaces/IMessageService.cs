using ChatApp.Common.Dtos.Message;
using ChatApp.Server.Entities;

namespace ChatApp.Server.Services.Interfaces;

public interface IMessageService : IBaseService
{
    public Task<List<MessageDto>> GetMessages(long userId, CancellationToken cancellationToken);
}