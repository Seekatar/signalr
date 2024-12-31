using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

// use this at SignalR's userId
public class SignalRUserNameProvider : IUserIdProvider
{
    public virtual string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst(ClaimTypes.Actor)?.Value;
    }
}