using Domain.Abstracts;
using Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public interface ILivestock
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        string BatchNumber { get; set; }
        bool BeingManaged { get; set; }
        DateTime Birthdate { get; set; }
        string BirthDefect { get; set; }
        bool BornDefective { get; set; }
        bool BottleFed { get; set; }
        string Description { get; set; }
        long? FatherId { get; set; }
        bool ForSale { get; set; }
        string Gender { get; set; }
        bool InMilk { get; set; }
        long LivestockBreedId { get; set; }
        long? LocationId { get; set; }
        long? MotherId { get; set; }
        string Name { get; set; }
        long? StatusId { get; set; }
        bool Sterile { get; set; }
        string Variety { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class Livestock : BaseEntity, ILivestock
    {
        public Livestock(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }

        [Required][ForeignKey("Breed")] public long LivestockBreedId { get; set; }
        [ForeignKey("Mother")] public long? MotherId { get; set; }
        [ForeignKey("Father")] public long? FatherId { get; set; }
        [Required][ForeignKey("Status")] public long? StatusId { get; set; }
        [ForeignKey("Location")] public long? LocationId { get; set; }
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required][MaxLength(40)] public string BatchNumber { get; set; }
        [Required] public DateTime Birthdate { get; set; }
        [Required][MaxLength(1)] public string Gender { get; set; }
        [MaxLength(40)] public string Variety { get; set; }
        [Required][MaxLength(255)] public string Description { get; set; }
        [RequiredIf("BornDefective", true)][MaxLength(255)] public string BirthDefect { get; set; }
        [Required] public bool Sterile { get; set; }
        [Required] public bool InMilk { get; set; }
        [Required] public bool BottleFed { get; set; }
        [Required] public bool ForSale { get; set; }
        [Required] public bool BeingManaged { get; set; }
        [Required] public bool BornDefective { get; set; }
        public virtual LivestockBreed Breed { get; set; }
        public virtual Livestock? Mother { get; set; }
        public virtual Livestock? Father { get; set; }
        public virtual LivestockStatus? Status { get; set; }
        public virtual LandPlot? Location { get; set; }

        [NotMapped]
        public virtual ICollection<ScheduledDuty> ScheduledDuties { get; set; } = new HashSet<ScheduledDuty>();

        [NotMapped]
        [ForeignKey("ScheduledDuties"), Column(nameof(Recipient))]
        public string Recipient => nameof(Livestock);

        [NotMapped]
        [ForeignKey("ScheduledDuties"), Column(nameof(RecipientId))]
        public long RecipientId => Id;
    }
}
