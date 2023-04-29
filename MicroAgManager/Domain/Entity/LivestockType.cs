using Domain.Abstracts;

namespace Domain.Entity
{
    public class LivestockType : BaseEntity
    {
        public LivestockType(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string ParentMaleName { get; set; }
        public string ParentFemaleName { get; set; }
        public string DefaultStatus { get; set; }
        public string Care { get; set; }
        public ICollection<LivestockBreed> Breeds { get; set; } = new List<LivestockBreed>();
        public ICollection<LivestockStatus> Statuses { get; set; } = new List<LivestockStatus>();
        public ICollection<LivestockFeed> Feeds { get; set; } = new List<LivestockFeed>();
    }
}
