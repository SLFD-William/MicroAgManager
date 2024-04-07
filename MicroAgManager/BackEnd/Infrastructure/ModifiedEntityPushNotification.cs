using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;

namespace BackEnd.Infrastructure
{


    public class ModifiedEntityPushNotification : ValueObject, INotification, IModifiedEntityPushNotification
    {
        public ModifiedEntityPushNotification()
        {
        }

        public ModifiedEntityPushNotification(Guid tenantId, string entityModel, string modelType,DateTime serverModifiedTime)
        {
            TenantId = tenantId;
            ModelJson = entityModel ?? throw new ArgumentNullException(nameof(entityModel));
            ModelType = modelType;
            ServerModifiedTime = serverModifiedTime;
        }

        public Guid TenantId { get; set; }
        public string ModelJson { get; set; }
        public string ModelType { get; set; }
        public DateTime ServerModifiedTime { get; set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TenantId;
            yield return ModelJson;
            yield return ModelType;
            yield return ServerModifiedTime;
        }
    }
}
