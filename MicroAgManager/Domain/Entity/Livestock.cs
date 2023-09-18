using Domain.Abstracts;
using Domain.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class Livestock : BaseEntity
    {
        public Livestock(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }

        [Required][ForeignKey("Breed")] public long LivestockBreedId { get; set; }
        [ForeignKey("Mother")] public long? MotherId { get; set; }
        [ForeignKey("Father")] public long? FatherId { get; set; }


        [Required] [MaxLength(40)]public string Name { get; set; }
        [Required][MaxLength(40)] public string BatchNumber { get; set; }
        [Required] public DateTime Birthdate { get; set; }
        [Required][MaxLength(1)] public string Gender { get; set; }
        [MaxLength(40)] public string Variety { get; set; }
        [Required][MaxLength(255)] public string Description { get; set; }
        [RequiredIf("BornDefective",true)] [MaxLength(255)] public string BirthDefect { get; set; }
        [Required] public bool Sterile { get; set; }
        [Required] public bool InMilk { get; set; }
        [Required] public bool BottleFed { get; set; }
        [Required] public bool ForSale { get; set; }
        [Required] public bool BeingManaged { get; set; }
        [Required] public bool BornDefective { get; set; }

         public virtual ICollection<LandPlot> Locations { get; set; } = new List<LandPlot>();
         public virtual ICollection<LivestockStatus> Statuses { get; set; } = new List<LivestockStatus>();
        public virtual ICollection<ScheduledDuty> ScheduledDuties { get; set; } = new List<ScheduledDuty>();
        public virtual LivestockBreed Breed { get; set; }
        public virtual Livestock? Mother { get; set; }
        public virtual Livestock? Father { get; set; }
    }
}
