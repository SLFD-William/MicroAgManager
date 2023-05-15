using BackEnd.Infrastructure;

namespace BackEnd.Hubs
{
    public interface INotificationClient //need this interface to be used by signalR in NotificationHub.cs
    {
        Task ReceiveEntitiesModifiedMessage(EntitiesModifiedNotification modifications);
    }
}
