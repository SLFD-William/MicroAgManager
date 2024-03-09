using Domain.Abstracts;
using Domain.Constants;
using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;

namespace Domain.Models
{
    public class TreatmentRecordModel :  BaseModel,ITreatmentRecord,IHasRecipient
    {
        [Required][ForeignKey("Treatment")] public long TreatmentId { get; set; }
        public virtual TreatmentModel Treatment { get; set; }
        public string? Notes { get; set; }
        [Required] public DateTime DatePerformed { get; set; }
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
    }
}
