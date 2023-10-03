using Domain.Abstracts;
using Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class Measure : BaseEntity
    {
        public Measure(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("Unit")] public long UnitId { get; set; }
        [Required][MaxLength(20)] public string Method { get; set; } = nameof(MeasurementMethodConstants.Direct);
        [Required][MaxLength(40)] public string Name { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
