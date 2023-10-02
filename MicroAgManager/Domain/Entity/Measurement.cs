using Domain.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entity
{
    public class Measurement : BaseEntity
    {
        public Measurement(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("Measure")] public long MeasureId { get; set; }
        public virtual Measure Measure { get; set; }
        [Required] public long RecipientTypeId { get; set; }
        [Required][MaxLength(40)] public string RecipientType { get; set; }
        [Required] public long RecipientId { get; set; }
        [Precision(18, 3)][Required] public decimal Value { get; set; }
        [Required] public string MeasurementUnit { get; set; } =nameof(MeasurementUnitConstants.Undefined);
        public string Notes { get; set; }
        [Required] public DateTime DatePerformed { get; set; }

    }
}
