using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using server.Hubs;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        internal IHubContext<MessageHub> _hubContext { get; }

        public MessageController(ILogger<MessageController> logger, IHubContext<MessageHub> messageHub)
        {
            _logger = logger;
            _hubContext = messageHub;
        }

        [HttpGet]
        public async Task<IActionResult> Send()
        {
            _logger.LogInformation("About to send message");
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", new Message { Title = "My Title", Text = "Testing", Timestamp = DateTimeOffset.Now, SenderUsername = "Jim", Type = "INFO" });
            return Ok("test");
        }
    }
}
