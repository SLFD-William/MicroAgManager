using Domain.Abstracts;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class BreedingRecord : BaseEntity, IHasRecipient
    {
        public BreedingRecord(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        public long RecipientTypeId { get; set; }
        public string RecipientType { get; set; }
        [Required][ForeignKey("Female")] public required long RecipientId { get; set; }
        [ForeignKey("Male")] public long? MaleId { get; set; }
        public DateTime ServiceDate { get; set; }
        public DateTime? ResolutionDate { get; set; }
        
        public int? StillbornMales { get; set; }
        public int? StillbornFemales { get; set; }
        public int? BornMales { get; set; }
        public int? BornFemales { get; set; }
        [MaxLength(40)]public string? Resolution { get; set; }

        public string Notes { get; set; }
        public virtual Livestock? Male { get; set; }
        public virtual Livestock? Female { get; set; }

    }
}
