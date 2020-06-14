using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using server.Models;

namespace server.Hubs
{
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
        public Task SendMessage(Message message)
        {
            _logger.LogInformation($"Sending message with title {message.Title}");
            return Clients.All.ReceiveMessage(message);
        }
        public Task SendMessageToUser(string username, Message message)
        {
            return Clients.All.ReceiveMessage(username, message);
        }
    }
}