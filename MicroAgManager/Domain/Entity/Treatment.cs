using Domain.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;

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
        decimal Duration { get; set; }
      IUnit? DurationUnit { get; set; }
        long? DurationUnitId { get; set; }
        decimal Frequency { get; set; }
      IUnit? FrequencyUnit { get; set; }
        long? FrequencyUnitId { get; set; }
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


        [Precision(18, 3)] public decimal Frequency { get; set; }
        [ForeignKey(nameof(FrequencyUnit))] public long? FrequencyUnitId { get; set; }
        public virtual Unit? FrequencyUnit { get; set; }
        [Precision(18, 3)] public decimal Duration { get; set; }
        [ForeignKey(nameof(DurationUnit))] public long? DurationUnitId { get; set; }
        public virtual Unit? DurationUnit { get; set; }

         IUnit? ITreatment.DosageUnit { get => DosageUnit; set => DosageUnit = value as Unit; }
         IUnit? ITreatment.DurationUnit { get => DurationUnit; set => DurationUnit = value as Unit; }
         IUnit? ITreatment.FrequencyUnit { get => FrequencyUnit; set => FrequencyUnit = value as Unit; }
         IUnit? ITreatment.RecipientMassUnit { get => RecipientMassUnit; set => RecipientMassUnit = value as Unit; }
    }
}
