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

        public ModifiedEntityPushNotification(Guid tenantId, string entityModel, string modelType)
        {
            TenantId = tenantId;
            ModelJson = entityModel ?? throw new ArgumentNullException(nameof(entityModel));
            ModelType = modelType;
        }

        public Guid TenantId { get; set; }
        public string ModelJson { get; set; }
        public string ModelType { get; set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TenantId;
            yield return ModelJson;
            yield return ModelType;
        }
    }
}
