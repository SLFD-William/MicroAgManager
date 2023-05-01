using BackEnd.Models;
using Microsoft.AspNetCore.SignalR;

namespace BackEnd.Hubs
{
    // [Authorize(AuthenticationSchemes = "Bearer")]
    public class NotificationHub : Hub<INotificationClient>
    {
        //create signalR group for each tenant
        public async Task JoinGroup(Guid tenantId)
        {
            if (tenantId == Guid.NewGuid()) return;
            await Groups.AddToGroupAsync(Context.ConnectionId, tenantId.ToString());
        }
        public async Task LeaveGroup(Guid tenantId)
        {
            if(tenantId==Guid.NewGuid()) return;
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, tenantId.ToString());
        }
        public async Task SendEntitiesModifiedNotification(Guid tenantId, EntitiesModifiedNotification modifications)
        {
            await Clients.Group(tenantId.ToString()).ReceiveEntitiesModifiedMessage(modifications);
        }
           
    }
}