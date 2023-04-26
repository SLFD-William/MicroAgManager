using Microsoft.AspNetCore.SignalR;

namespace BackEnd.Hubs
{
    // [Authorize(AuthenticationSchemes = "Bearer")]
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}