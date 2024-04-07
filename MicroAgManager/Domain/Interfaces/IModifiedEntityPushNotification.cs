namespace Domain.Interfaces
{
    public interface IModifiedEntityPushNotification
    {
        string ModelJson { get; set; }
        string ModelType { get; set; }
        Guid TenantId { get; set; }
        DateTime ServerModifiedTime { get; set; }
    }
}
