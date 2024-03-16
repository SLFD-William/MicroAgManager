using Domain.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public interface ITreatment
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        string BrandName { get; set; }
        decimal DosageAmount { get; set; }
      IUnit? DosageUnit { get; set; }
        long? DosageUnitId { get; set; }
        string LabelMethod { get; set; }
        int MeatWithdrawal { get; set; }
        int MilkWithdrawal { get; set; }
        string Name { get; set; }
        string Reason { get; set; }
        decimal RecipientMass { get; set; }
      IUnit? RecipientMassUnit { get; set; }
        long? RecipientMassUnitId { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class Treatment : BaseEntity, ITreatment, IHasFrequencyAndDuration
    {
        public Treatment(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string Reason { get; set; }
        public string LabelMethod { get; set; } = TreatmentConstants.Grooming;
        public int MeatWithdrawal { get; set; } = 0;
        public int MilkWithdrawal { get; set; } = 0;

        [Precision(18, 3)] public decimal DosageAmount { get; set; } = 0;
        [ForeignKey(nameof(DosageUnit))] public long? DosageUnitId { get; set; }
        public virtual Unit? DosageUnit { get; set; }

        [Precision(18, 3)] public decimal RecipientMass { get; set; } = 0;
        [ForeignKey(nameof(RecipientMassUnit))] public long? RecipientMassUnitId { get; set; }
        public virtual Unit? RecipientMassUnit { get; set; }
        [Required][Precision(18, 3)] public decimal DurationScalar { get; set; } = 1;
        [Required][ForeignKey(nameof(DurationUnit))] public long? DurationUnitId { get; set; }
        [Required] public virtual Unit? DurationUnit { get; set; }

        [Required][Precision(18, 3)] public decimal PerScalar { get; set; } = 1;
        [Required][ForeignKey(nameof(PerUnit))] public long? PerUnitId { get; set; }
        [Required] public virtual Unit? PerUnit { get; set; }
        [Required][Precision(18, 3)] public decimal EveryScalar { get; set; }=1;
        [Required][ForeignKey(nameof(EveryUnit))] public long? EveryUnitId { get; set; }
        [Required] public virtual Unit? EveryUnit { get; set; }


        IUnit? ITreatment.DosageUnit { get => DosageUnit; set => DosageUnit = value as Unit; }
         IUnit? ITreatment.RecipientMassUnit { get => RecipientMassUnit; set => RecipientMassUnit = value as Unit; }
    }
}
