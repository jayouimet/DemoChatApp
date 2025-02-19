using ChatApp.Common.Dtos.Message;
using ChatApp.Common.Dtos.User;

namespace ChatApp.Common.Hubs.Interfaces;

public interface IChatHubClient
{
    Task ConnectedUsersList(IEnumerable<UserDto> users);
    Task UserConnected(UserDto user);
    Task OnMessageSent(MessageDto message);
}