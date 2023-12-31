using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class Chore : BaseEntity
    {
        public Chore(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required] public long RecipientTypeId { get; set; }
        [Required][MaxLength(40)] public string RecipientType { get; set; }
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required][MaxLength(40)] public string Color { get; set; } = "transparent";
        //chore is due by noon once a day,
        //chode is due by 6:00am 2x a week every 2.5 days
        [Required] public TimeSpan DueByTime { get; set; } = new TimeSpan(12, 0, 0);
        [Required][Precision(18, 3)] public decimal Frequency { get; set; } = 1;
        [Required][ForeignKey(nameof(FrequencyUnit))] public long FrequencyUnitId { get; set; }
        [Required] public virtual Unit FrequencyUnit { get; set; }
        [Precision(18, 3)] public decimal Period { get; set; } = 1;
        [ForeignKey(nameof(PeriodUnit))] public long? PeriodUnitId { get; set; }
        public virtual Unit? PeriodUnit { get; set; }
        public virtual ICollection<Duty> Duties { get; set; } = new List<Duty>();
        
    }
}
