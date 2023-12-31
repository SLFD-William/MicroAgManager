using BackEnd.Hubs;
using BackEnd.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace MicroAgManager.API.Hubs
{
    public class NotificationHub : Hub<INotificationClient>
    {
        //public async Task Handle(EntitiesModifiedNotification notification, CancellationToken cancellationToken)
        //{
        //    if (notification.TenantId == Guid.NewGuid()) return;
        //    await Clients.Group(notification.TenantId.ToString()).ReceiveEntitiesModifiedMessage(notification);
        //}

        //create signalR group for each tenant
        public async Task JoinGroup(Guid tenantId)
        {
            if (tenantId == Guid.NewGuid()) return;
            await Groups.AddToGroupAsync(Context.ConnectionId, tenantId.ToString());
        }
        public async Task LeaveGroup(Guid tenantId)
        {
            if (tenantId == Guid.NewGuid()) return;
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, tenantId.ToString());
        }
    }
    public class NotificationHandler : INotificationHandler<EntitiesModifiedNotification>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationHandler(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Handle(EntitiesModifiedNotification notification, CancellationToken cancellationToken)
        {
            if (notification.TenantId == Guid.NewGuid()) return;
            await _hubContext.Clients.Group(notification.TenantId.ToString()).SendAsync("ReceiveEntitiesModifiedMessage", notification);
        }
    }
}