using Domain.ValueObjects;
using MediatR;

namespace BackEnd.Infrastructure
{
    public class EntitiesModifiedNotification : ValueObject, INotification
    {
        public EntitiesModifiedNotification()
        {
        }

        public EntitiesModifiedNotification(Guid tenantId, List<ModifiedEntity> entitiesModified)
        {
            TenantId = tenantId;
            EntitiesModified = entitiesModified;
        }

        public Guid TenantId { get; set; }
        public List<ModifiedEntity> EntitiesModified { get; set; } = new();

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TenantId;
            yield return EntitiesModified;
        }
    }
}
