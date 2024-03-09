using Domain.Abstracts;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public interface IBreedingRecord
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        int? BornFemales { get; set; }
        int? BornMales { get; set; }
        long? MaleId { get; set; }
        string? Notes { get; set; }
        long FemaleId { get; set; }
        string? Resolution { get; set; }
        DateTime? ResolutionDate { get; set; }
        DateTime ServiceDate { get; set; }
        int? StillbornFemales { get; set; }
        int? StillbornMales { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class BreedingRecord : BaseEntity, IHasRecipient, IBreedingRecord
    {
        public BreedingRecord(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("Female")] public required long FemaleId { get; set; }
        [ForeignKey("Male")] public long? MaleId { get; set; }
        public DateTime ServiceDate { get; set; }
        public DateTime? ResolutionDate { get; set; }

        public int? StillbornMales { get; set; }
        public int? StillbornFemales { get; set; }
        public int? BornMales { get; set; }
        public int? BornFemales { get; set; }
        [MaxLength(40)] public string? Resolution { get; set; }

        public string? Notes { get; set; }
        public virtual Livestock? Male { get; set; }
        public virtual Livestock? Female { get; set; }
        public long RecipientTypeId { get; set; }
        public string RecipientType { get; set; }
        [NotMapped]long IHasRecipient.RecipientId { get => FemaleId; set => FemaleId=value; }
    }
}
