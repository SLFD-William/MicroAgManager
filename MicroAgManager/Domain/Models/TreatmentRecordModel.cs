using Domain.Abstracts;
using Domain.Constants;
using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    public class TreatmentRecordModel :  BaseHasRecipientModel
    {
        [Required][ForeignKey("Treatment")] public long TreatmentId { get; set; }
        public virtual TreatmentModel Treatment { get; set; }
        public string Notes { get; set; }
        [Required] public DateTime DatePerformed { get; set; }
        [Precision(18, 3)] public decimal DosageAmount { get; set; } = 0;
        [Required][ForeignKey(nameof(DosageUnit))] public long DosageUnitId { get; set; }
        public virtual UnitModel DosageUnit { get; set; }
        public string AppliedMethod { get; set; } = TreatmentConstants.Grooming;

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
