using Domain.Abstracts;

namespace Domain.Entity
{
    public class Livestock : BaseEntity
    {
        public Livestock(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        public LivestockBreed Breed { get; set; }
        public long? MotherId { get; set; }
        public long? FatherId { get; set; }
        public Livestock? Mother { get; set; }
        public Livestock? Father { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public string Gender { get; set; }
        public string Variety { get; set; }
        public string Description { get; set; }
        public bool BeingManaged { get; set; }
        public bool BornDefective { get; set; }
        public string BirthDefect { get; set; }
        public bool Sterile { get; set; }
        public bool InMilk { get; set; }
        public bool BottleFed { get; set; }
        public bool ForSale { get; set; }
        public ICollection<LivestockStatus> Statuses { get; set; } = new List<LivestockStatus>();
        public ICollection<LandPlot> Locations { get; set; } = new List<LandPlot>();
        public ICollection<ScheduledDuty> ScheduledDuties { get; set; } = new List<ScheduledDuty>();

    }
}
