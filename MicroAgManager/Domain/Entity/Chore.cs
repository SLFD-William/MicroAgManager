using Domain.Abstracts;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public interface IChore
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        string Color { get; set; }
        TimeSpan DueByTime { get; set; }
        ICollection<IDuty>? Duties { get; set; }
        string Name { get; set; }
        string RecipientType { get; set; }
        long RecipientTypeId { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class Chore : BaseEntity, IChore,IHasFrequencyAndDuration
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
        public virtual ICollection<Duty> Duties { get; set; } = new List<Duty>();
        [Precision(18, 3)] public decimal DurationScalar { get; set; } = 0;
        public long? DurationUnitId { get; set; }
        [Required][Precision(18, 3)] public decimal PerScalar { get; set; } = 1;
        [Required][ForeignKey(nameof(PerUnit))] public long? PerUnitId { get; set; }
        [Required] public virtual Unit? PerUnit { get; set; }
        [Required][Precision(18, 3)] public decimal EveryScalar { get; set; } = 1;
        [Required][ForeignKey(nameof(EveryUnit))] public long? EveryUnitId { get; set; }
        [Required] public virtual Unit? EveryUnit { get; set; }
        ICollection<IDuty>? IChore.Duties { get => Duties as ICollection<IDuty>; set => Duties = value as ICollection<Duty> ??  new List<Duty>();}
       
    }
}
