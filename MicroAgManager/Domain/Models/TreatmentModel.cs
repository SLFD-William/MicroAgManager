using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    public class TreatmentModel : BaseModel
    {
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required][MaxLength(40)] public string BrandName { get; set; }
        [Required][MaxLength(40)] public string Reason { get; set; }
        [Required][MaxLength(20)] public string LabelMethod { get; set; } = TreatmentConstants.Grooming;
        [Required] public int MeatWithdrawal { get; set; } = 0;
        [Required] public int MilkWithdrawal { get; set; } = 0;
        [Required][Precision(18, 3)] public decimal DosageAmount { get; set; } = 0;
        [ForeignKey(nameof(DosageUnit))] public long? DosageUnitId { get; set; }
        public virtual UnitModel? DosageUnit { get; set; }

        [Required][Precision(18, 3)] public decimal RecipientMass { get; set; } = 0;
        [ForeignKey(nameof(RecipientMassUnit))] public long? RecipientMassUnitId { get; set; }
        public virtual UnitModel? RecipientMassUnit { get; set; }


        [Required][Precision(18, 3)] public decimal Frequency { get; set; } = 0;
        [ForeignKey(nameof(FrequencyUnit))] public long? FrequencyUnitId { get; set; }
        public virtual UnitModel? FrequencyUnit { get; set; }
        [Required][Precision(18, 3)] public decimal Duration { get; set; } = 0;
        [ForeignKey(nameof(DurationUnit))] public long? DurationUnitId { get; set; }
        public virtual UnitModel? DurationUnit { get; set; }


        public static TreatmentModel Create(Treatment treatment)
        {
            var model = PopulateBaseModel(treatment, new TreatmentModel
            { 
                Name = treatment.Name,
                BrandName = treatment.BrandName,
                Reason = treatment.Reason,
                LabelMethod = treatment.LabelMethod,
                MeatWithdrawal = treatment.MeatWithdrawal,
                MilkWithdrawal = treatment.MilkWithdrawal,
                DosageAmount = treatment.DosageAmount,
                DosageUnitId = treatment.DosageUnitId,
                RecipientMass = treatment.RecipientMass,
                RecipientMassUnitId = treatment.RecipientMassUnitId,
                Frequency = treatment.Frequency,
                FrequencyUnitId = treatment.FrequencyUnitId,
                Duration = treatment.Duration,
                DurationUnitId = treatment.DurationUnitId
                }) as TreatmentModel;
            return model;
        }

        public override BaseModel Map(BaseModel model)
        {
            if (model == null || model is not TreatmentModel) return null;
            ((TreatmentModel)model).Name = Name;
            ((TreatmentModel)model).BrandName = BrandName;
            ((TreatmentModel)model).Reason = Reason;
            ((TreatmentModel)model).LabelMethod = LabelMethod;
            ((TreatmentModel)model).MeatWithdrawal = MeatWithdrawal;
            ((TreatmentModel)model).MilkWithdrawal = MilkWithdrawal;
            ((TreatmentModel)model).DosageAmount = DosageAmount;
            ((TreatmentModel)model).DosageUnitId = DosageUnitId;
            ((TreatmentModel)model).RecipientMass = RecipientMass;
            ((TreatmentModel)model).RecipientMassUnitId = RecipientMassUnitId;
            ((TreatmentModel)model).Frequency = Frequency;
            ((TreatmentModel)model).FrequencyUnitId = FrequencyUnitId;
            ((TreatmentModel)model).Duration = Duration;
            ((TreatmentModel)model).DurationUnitId = DurationUnitId;
            ((TreatmentModel)model).EntityModifiedOn = EntityModifiedOn;
            return model;
        }

        public override BaseEntity Map(BaseEntity entity)
        {
            if (entity == null || entity is not Treatment) return null;
            ((Treatment)entity).Name = Name;
            ((Treatment)entity).BrandName = BrandName;
            ((Treatment)entity).Reason = Reason;
            ((Treatment)entity).LabelMethod = LabelMethod;
            ((Treatment)entity).MeatWithdrawal = MeatWithdrawal;
            ((Treatment)entity).MilkWithdrawal = MilkWithdrawal;
            ((Treatment)entity).DosageAmount = DosageAmount;
            ((Treatment)entity).DosageUnitId = DosageUnitId;
            ((Treatment)entity).RecipientMass = RecipientMass;
            ((Treatment)entity).RecipientMassUnitId = RecipientMassUnitId;
            ((Treatment)entity).Frequency = Frequency;
            ((Treatment)entity).FrequencyUnitId = FrequencyUnitId;
            ((Treatment)entity).Duration = Duration;
            ((Treatment)entity).DurationUnitId = DurationUnitId;
            ((Treatment)entity).ModifiedOn = DateTime.UtcNow;
            return entity;
        }
    }
}
