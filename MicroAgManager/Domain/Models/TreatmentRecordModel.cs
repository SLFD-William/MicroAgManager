using Domain.Abstracts;
using Domain.Constants;
using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Domain.Logic;

namespace Domain.Models
{
    public class TreatmentRecordModel :  BaseModel,ITreatmentRecord,IHasRecipientModel
    {   
        [Required][ForeignKey("Treatment")] public long TreatmentId { get; set; }
        public virtual TreatmentModel Treatment { get; set; }
        public string? Notes { get; set; }
        [Required] public DateTime DatePerformed { get; set; } = DateTime.MinValue;
        [Precision(18, 3)] public decimal? DosageAmount { get; set; }
        [ForeignKey(nameof(DosageUnit))] public long? DosageUnitId { get; set; }
        public virtual UnitModel DosageUnit { get; set; }
        public string AppliedMethod { get; set; } = TreatmentConstants.Grooming;
        public long RecipientTypeId { get; set; }
        public string RecipientType { get; set; }
        public long RecipientId { get; set; }
         DateTime ITreatmentRecord.ModifiedOn { get => EntityModifiedOn; set => EntityModifiedOn = value== EntityModifiedOn ? EntityModifiedOn: EntityModifiedOn; }
         IUnit ITreatmentRecord.DosageUnit { get => DosageUnit; set => DosageUnit = value as UnitModel ?? DosageUnit; }

        public static TreatmentRecordModel Create(TreatmentRecord treatmentRecord)
        {
            var model = PopulateBaseModel(treatmentRecord, new TreatmentRecordModel
            {
                TreatmentId = treatmentRecord.TreatmentId,
                Notes = treatmentRecord.Notes,
                DatePerformed = treatmentRecord.DatePerformed,
                DosageAmount = treatmentRecord.DosageAmount,
                DosageUnitId = treatmentRecord.DosageUnitId,
                AppliedMethod = treatmentRecord.AppliedMethod
            }) as TreatmentRecordModel;
            return model;
        }
        public override BaseModel Map(BaseModel model)
        {
            if (model == null || model is not TreatmentRecordModel) return null;
            ((TreatmentRecordModel)model).TreatmentId = TreatmentId;
            ((TreatmentRecordModel)model).RecipientTypeId = RecipientTypeId;
            ((TreatmentRecordModel)model).RecipientType = RecipientType;
            ((TreatmentRecordModel)model).RecipientId = RecipientId;
            ((TreatmentRecordModel)model).Notes = Notes;
            ((TreatmentRecordModel)model).DatePerformed = DatePerformed;
            ((TreatmentRecordModel)model).DosageAmount = DosageAmount;
            ((TreatmentRecordModel)model).DosageUnitId = DosageUnitId;
            ((TreatmentRecordModel)model).AppliedMethod = AppliedMethod;
            ((TreatmentRecordModel)model).EntityModifiedOn = EntityModifiedOn;
            return model;
        }

        public override BaseEntity Map(BaseEntity entity)
        {
            if (entity == null || entity is not TreatmentRecord) return null;
            ((TreatmentRecord)entity).TreatmentId = TreatmentId;
            ((TreatmentRecord)entity).RecipientTypeId = RecipientTypeId;
            ((TreatmentRecord)entity).RecipientType = RecipientType;
            ((TreatmentRecord)entity).RecipientId = RecipientId;
            ((TreatmentRecord)entity).Notes = Notes;
            ((TreatmentRecord)entity).DatePerformed = DatePerformed;
            ((TreatmentRecord)entity).DosageAmount = DosageAmount;
            ((TreatmentRecord)entity).DosageUnitId = DosageUnitId;
            ((TreatmentRecord)entity).AppliedMethod = AppliedMethod;
            ((TreatmentRecord)entity).ModifiedOn = DateTime.UtcNow;
            return entity;
        }
        public void PopulateDynamicRelations(DbContext genericContext) => RecipientLogic.PopulateDynamicRelations(genericContext, this);
        [NotMapped] public string RecipientTypeItem { get; set; } = string.Empty;
        [NotMapped] public string RecipientItem { get; set; } = string.Empty;
        [NotMapped] public string TreatmentName { get => Treatment?.Name ?? string.Empty; }
        [NotMapped] public string TreatmentBrandName { get => Treatment?.BrandName ?? string.Empty; }
        [NotMapped] public string TreatmentReason { get => Treatment?.Reason ?? string.Empty; }
        [NotMapped] public string TreatmentLabelMethod { get => Treatment?.LabelMethod ?? string.Empty; }
        [NotMapped] public decimal TreatmentDosage { get => Treatment?.DosageAmount ?? 0; }
        [NotMapped] public string TreatmentDosageUnit { get =>(Treatment?.DosageUnit is UnitModel) ? $"{Treatment?.DosageUnit?.Name} ({Treatment?.DosageUnit?.Symbol})" : string.Empty; }
        [NotMapped] public decimal TreatmentRecipientMass { get => Treatment?.RecipientMass ?? 0; }
        [NotMapped] public string TreatmentRecipientMassUnit { get => (Treatment?.RecipientMassUnit is UnitModel) ? $"{Treatment?.RecipientMassUnit?.Name} ({Treatment?.DosageUnit?.Symbol})" : string.Empty; }
        [NotMapped] public int TreatmentMeatWithdrawal { get => Treatment?.MeatWithdrawal ?? 0; }
        [NotMapped] public int TreatmentMilkWithdrawal { get => Treatment?.MilkWithdrawal ?? 0; }
        [NotMapped] public decimal TreatmentPer { get => Treatment?.PerScalar ?? 0; }
        [NotMapped] public string TreatmentPerUnit { get => (Treatment?.PerUnit is UnitModel) ? $"{Treatment?.PerUnit?.Name} ({Treatment?.PerUnit?.Symbol})" : string.Empty; }
        [NotMapped] public decimal TreatmentEvery { get => Treatment?.EveryScalar ?? 0; }
        [NotMapped] public string TreatmentEveryUnit { get => (Treatment?.EveryUnit is UnitModel) ? $"{Treatment?.EveryUnit?.Name} ({Treatment?.EveryUnit?.Symbol})" : string.Empty; }
        [NotMapped] public decimal TreatmentDuration { get => Treatment?.DurationScalar ?? 0; }
        [NotMapped] public string TreatmentDurationUnit { get => (Treatment?.DurationUnit is UnitModel) ? $"{Treatment?.DurationUnit?.Name} ({Treatment?.DurationUnit?.Symbol})" : string.Empty; }
        [NotMapped] public string AppliedUnit { get => DosageUnit?.Name ?? string.Empty; }
    }
}
