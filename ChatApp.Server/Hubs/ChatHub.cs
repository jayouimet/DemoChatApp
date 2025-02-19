using ChatApp.Common.Dtos.Message;
using ChatApp.Common.Dtos.User;
using ChatApp.Common.Hubs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Server.Hubs;

[Authorize]
public class ChatHub : Hub<IChatHubClient>, IChatHubServer
{
    private static readonly Dictionary<int, string> Connections = new Dictionary<int, string>();
    private static readonly IDictionary<int, UserDto> ConnectedUsers = new Dictionary<int, UserDto>();
    private static readonly ICollection<MessageDto> Messages = new List<MessageDto>();
    
    public ChatHub()
    {
        
    }

    public async Task SendMessage(MessageDto message)
    {
        Messages.Add(message);
        
        await Clients.Caller.OnMessageSent(message);
        
        if (Connections.TryGetValue(message.ToUserId, out var toConnectionId))
        {
            await Clients.Client(toConnectionId).OnMessageSent(message);
        }
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public async Task ConnectUser(UserDto user)
    {
        await Clients.Caller.ConnectedUsersList(ConnectedUsers.Values);
        
        if (ConnectedUsers.TryAdd(user.Id, user))
        {
            Connections.Add(user.Id, Context.ConnectionId);
            await Clients.Others.UserConnected(user);
        }
    }
}