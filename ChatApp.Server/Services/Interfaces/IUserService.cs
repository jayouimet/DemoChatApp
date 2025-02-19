using ChatApp.Common.Dtos.User;
using ChatApp.Server.Entities;

namespace ChatApp.Server.Services.Interfaces;

public interface IUserService : IBaseService
{
    public Task<List<UserDto>> GetContacts(CancellationToken cancellationToken);
}