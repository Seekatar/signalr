using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using server.Models;

namespace server.Hubs;

public interface IMessageClient
{
    Task ReceiveMessage(Message message);
    Task ReceiveMessage(string username, Message message);
}

public class MessageHub : Hub<IMessageClient>
{
    private readonly ILogger _logger;

    public MessageHub(ILogger<MessageHub> logger)
    {
        _logger = logger;
    }
    public override async Task OnConnectedAsync()
    {
        var group = "";
        var clientCode = Context.User?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GroupSid)?.Value;
        if (!string.IsNullOrEmpty(clientCode))
        {
            group = $" with group of {clientCode}";
            await Groups.AddToGroupAsync(Context.ConnectionId, clientCode);
        }
        _logger.LogInformation("Connected: {userId}{group}", Context.UserIdentifier, group);
        await base.OnConnectedAsync();
    }
}