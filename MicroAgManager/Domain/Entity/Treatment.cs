using Domain.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entity
{
    public class Treatment : BaseEntity
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

        [Precision(18,3)]public decimal DosageAmount { get; set; } = 0;
        [Required][ForeignKey(nameof(DosageUnit))] public long DosageUnitId { get; set; }
        public virtual Unit DosageUnit { get; set; }

        [Precision(18, 3)] public decimal AnimalMass { get; set; } = 0;
        [Required][ForeignKey(nameof(AnimalMassUnit))] public long AnimalMassUnitId { get; set; }
        public virtual Unit AnimalMassUnit { get; set; }


        [Precision(18, 3)] public decimal Frequency { get; set; }
        [Required][ForeignKey(nameof(FrequencyUnit))] public long FrequencyUnitId { get; set; }
        public virtual Unit FrequencyUnit { get; set; }
        [Precision(18, 3)] public decimal Duration { get; set; }
        [Required][ForeignKey(nameof(DurationUnit))] public long DurationUnitId { get; set; }
        public virtual Unit DurationUnit { get; set; }

    }
}
