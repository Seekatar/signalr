using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using server.Hubs;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly AppSettings _appSettings;

        internal IHubContext<MessageHub> _hubContext { get; }

        public MessageController(ILogger<MessageController> logger, IHubContext<MessageHub> messageHub, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _hubContext = messageHub;
            _appSettings = appSettings.Value;
        }

        [HttpGet("jwt")]
        [AllowAnonymous]
        public IActionResult GetJwt([FromQuery] string userId = "fflintstone")
        {
            _logger.LogDebug($"Getting jwt for {userId}");
            return Ok(FakeJwt(userId));
        }

        private object FakeJwt(string userId)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId),
                    new Claim(ClaimTypes.Actor, userId),
                    new Claim(ClaimTypes.Email, $"{userId}@rockhead.com"),
                    // this is what SignalR uses as ClaimsPrincipal by default, you can change the default
                    // with an IUserIdProvider
                    // https://docs.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-3.1#use-claims-to-customize-identity-handling
                    new Claim(ClaimTypes.NameIdentifier, userId)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpGet]
        public async Task<IActionResult> Send([FromQuery] string msg = "", [FromQuery] string type = Message.Information)
        {
            _logger.LogDebug($"About to send message: '{msg}' of type {type}");
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", new Message { Title = "My Title", Text = msg, Timestamp = DateTimeOffset.Now, SenderUsername = "Server", Type = type });
            return Ok($"Sent test message with Test = '{msg}'");
        }

        [HttpGet("send-to")]
        public async Task<IActionResult> SendTo([FromQuery] string userId, [FromQuery] string msg, [FromQuery] string type=Message.Information)
        {
            // always get proxy bad, even if no userId matches
            var proxy = _hubContext.Clients.User(userId);

            _logger.LogDebug($"About to send message to: '{userId}' of type {type}");

            await proxy.SendAsync("ReceiveMessage",
                            new Message
                            {
                                Title = "Info",
                                Text = $"Hello {userId}. {msg}",
                                Timestamp = DateTimeOffset.Now,
                                SenderUsername = userId,
                                Type = type
                            });

            return Ok($"Sent test to '{userId}'");
        }

    }
}
