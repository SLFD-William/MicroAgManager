using Domain.Abstracts;

namespace Domain.Entity
{
    public class LivestockBreed : BaseEntity
    {
        public LivestockBreed(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        public LivestockType Livestock { get; set; }
        public string Name { get; set; }
        public string EmojiChar { get; set; }
        public int GestationPeriod { get; set; }
        public int HeatPeriod { get; set; }
        public ICollection<Livestock> Livestocks { get; set; } = new List<Livestock>();
    }
}
