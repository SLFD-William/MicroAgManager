using Domain.Abstracts;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class TreatmentRecord : BaseEntity
    {
        public TreatmentRecord(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("Treatment")] public long TreatmentId { get; set; }
        public virtual Treatment Treatment { get; set; }
        [Required] public long RecipientTypeId { get; set; }
        [Required][MaxLength(40)] public string RecipientType { get; set; }
        [Required] public long RecipientId { get; set; }
        public string Notes { get; set; }
        [Required] public DateTime DatePerformed { get; set; }
        [Precision(18, 3)] public decimal DosageAmount { get; set; } = 0;
        [Required][ForeignKey(nameof(DosageUnit))] public long DosageUnitId { get; set; }
        public virtual Unit DosageUnit { get; set; }
        public string AppliedMethod { get; set; } = TreatmentConstants.Grooming;
    }
}
