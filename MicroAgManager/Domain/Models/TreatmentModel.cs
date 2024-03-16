using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;

namespace Domain.Models
{
    public class TreatmentModel : BaseModel,ITreatment,IHasFrequencyAndDuration
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

        [Required] public decimal DurationScalar { get; set; } = 1;
        [ForeignKey(nameof(DurationUnit))][Required] public long? DurationUnitId { get; set; }
        public virtual UnitModel? DurationUnit { get; set; }
        [Required][Precision(18, 3)] public decimal PerScalar { get; set; } = 1;
        [ForeignKey(nameof(PerUnit))][Required] public long? PerUnitId { get; set; }
        public virtual UnitModel? PerUnit { get; set; }
        [Required][Precision(18, 3)] public decimal EveryScalar { get; set; } = 1;
        [ForeignKey(nameof(EveryUnit))][Required] public long? EveryUnitId { get; set; }
        public virtual UnitModel? EveryUnit { get; set; }
        [NotMapped] DateTime ITreatment.ModifiedOn { get => EntityModifiedOn; set => EntityModifiedOn = value== EntityModifiedOn ? EntityModifiedOn: EntityModifiedOn; }
         IUnit? ITreatment.DosageUnit { get => DosageUnit; set => DosageUnit = value as UnitModel ?? DosageUnit; }
         IUnit? ITreatment.RecipientMassUnit { get => RecipientMassUnit; set => RecipientMassUnit = value as UnitModel ?? RecipientMassUnit; }

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
                DurationScalar=treatment.DurationScalar,
                DurationUnitId=treatment.DurationUnitId,
                PerScalar=treatment.PerScalar,
                PerUnitId=treatment.PerUnitId,
                EveryScalar=treatment.EveryScalar,
                EveryUnitId=treatment.EveryUnitId
      
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
            ((TreatmentModel)model).EntityModifiedOn = EntityModifiedOn;
            ((TreatmentModel)model).DurationUnitId = DurationUnitId;
            ((TreatmentModel)model).DurationScalar=DurationScalar;
            ((TreatmentModel)model).PerUnitId = PerUnitId;
            ((TreatmentModel)model).PerScalar = PerScalar;
            ((TreatmentModel)model).EveryUnitId = EveryUnitId;
            ((TreatmentModel)model).EveryScalar = EveryScalar;
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
            ((Treatment)entity).DurationUnitId = DurationUnitId;
            ((Treatment)entity).DurationScalar = DurationScalar;
            ((Treatment)entity).PerUnitId = PerUnitId;
            ((Treatment)entity).PerScalar = PerScalar;
            ((Treatment)entity).EveryUnitId = EveryUnitId;
            ((Treatment)entity).EveryScalar = EveryScalar;
            ((Treatment)entity).ModifiedOn = DateTime.UtcNow;
            return entity;
        }
    }
}
