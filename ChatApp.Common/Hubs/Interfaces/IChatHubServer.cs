using ChatApp.Common.Dtos.Message;
using ChatApp.Common.Dtos.User;

namespace ChatApp.Common.Hubs.Interfaces;

public interface IChatHubServer
{
    Task ConnectUser(UserDto user);
    Task SendMessage(MessageDto message);
}