using BackEnd.Hubs;
using BackEnd.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Host.Hubs
{
    // [Authorize(AuthenticationSchemes = "Bearer")]
    public class NotificationHub : Hub<INotificationClient>, INotificationHandler<EntitiesModifiedNotification>
    {
        public async Task Handle(EntitiesModifiedNotification notification, CancellationToken cancellationToken)
        {
            await Clients.Group(notification.TenantId.ToString()).ReceiveEntitiesModifiedMessage(notification); ;
        }

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
    }
}