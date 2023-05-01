using BackEnd.Hubs;
using BackEnd.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace BackEnd.BusinessLogic
{
    public class NotifyOnEntitiesModified : INotificationHandler<EntitiesModifiedNotification>
    {
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
        public NotifyOnEntitiesModified(IHubContext<NotificationHub, INotificationClient> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task Handle(EntitiesModifiedNotification notification, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.Group(notification.TenantId.ToString()).ReceiveEntitiesModifiedMessage(notification);
        }
    }
}
