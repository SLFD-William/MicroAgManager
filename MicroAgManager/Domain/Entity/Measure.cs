using Domain.Abstracts;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public interface IMeasure
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        string Method { get; set; }
        string Name { get; set; }
       IUnit Unit { get; set; }
        long UnitId { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class Measure : BaseEntity, IMeasure
    {
        public Measure(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("Unit")] public long UnitId { get; set; }
        [Required][MaxLength(20)] public string Method { get; set; } = nameof(MeasurementMethodConstants.Direct);
        [Required][MaxLength(40)] public string Name { get; set; }
        public virtual Unit Unit { get; set; }
      [NotMapped]  IUnit IMeasure.Unit { get => Unit; set =>Unit=value as Unit ?? Unit; }
    }
}
