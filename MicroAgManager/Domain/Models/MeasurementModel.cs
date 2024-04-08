using Domain.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Domain.Logic;

namespace Domain.Models
{
    public class MeasurementModel : BaseModel, IMeasurement, IHasRecipientModel
    {
        [Required][ForeignKey("Measure")] public long MeasureId { get; set; }
        public virtual MeasureModel Measure { get; set; }
        
        [Precision(18, 3)][Required] public decimal Value { get; set; }
        [Required][ForeignKey("MeasurementUnit")] public long MeasurementUnitId { get; set; }
        public virtual UnitModel MeasurementUnit { get; set; }
        public string? Notes { get; set; }
        [Required] public DateTime DatePerformed { get; set; } = DateTime.MinValue;
        public long RecipientTypeId { get; set; }
        public string RecipientType { get; set; }
        public long RecipientId { get; set; }
        [NotMapped] DateTime IMeasurement.ModifiedOn { get => EntityModifiedOn; set => EntityModifiedOn = value== EntityModifiedOn ? EntityModifiedOn: EntityModifiedOn; }
         IUnit IMeasurement.MeasurementUnit { get => MeasurementUnit; set => MeasurementUnit = value as UnitModel ?? MeasurementUnit; }

        public static MeasurementModel Create(Measurement measurement)
        {
            var model = PopulateBaseModel(measurement, new MeasurementModel
            {
                MeasureId = measurement.MeasureId,
                Value = measurement.Value,
                MeasurementUnitId = measurement.MeasurementUnitId,
                Notes = measurement.Notes,
                DatePerformed = measurement.DatePerformed,
                
            }) as MeasurementModel;
            return model;
        }

        public override BaseEntity Map(BaseEntity measurement)
        {
            if (measurement == null || measurement is not Measurement) return null;
            ((Measurement)measurement).MeasureId = MeasureId;
            ((Measurement)measurement).RecipientTypeId = RecipientTypeId;
            ((Measurement)measurement).RecipientType = RecipientType;
            ((Measurement)measurement).RecipientId = RecipientId;
            ((Measurement)measurement).Value = Value;
            ((Measurement)measurement).MeasurementUnitId = MeasurementUnitId;
            ((Measurement)measurement).Notes = Notes;
            ((Measurement)measurement).DatePerformed = DatePerformed;
            measurement.ModifiedOn = DateTime.UtcNow;
            return measurement;
        }

        public override BaseModel Map(BaseModel measurement)
        {
            if (measurement == null || measurement is not MeasurementModel) return null;
            ((MeasurementModel)measurement).MeasureId = MeasureId;
            ((MeasurementModel)measurement).Value = Value;
            ((MeasurementModel)measurement).MeasurementUnitId = MeasurementUnitId;
            ((MeasurementModel)measurement).Notes = Notes;
            ((MeasurementModel)measurement).DatePerformed = DatePerformed;
            ((MeasurementModel)measurement).EntityModifiedOn = EntityModifiedOn;
            ((MeasurementModel)measurement).RecipientId = RecipientId;
            ((MeasurementModel)measurement).RecipientTypeId = RecipientTypeId;
            ((MeasurementModel)measurement).RecipientType = RecipientType;
            return measurement;
        }
        public void PopulateDynamicRelations(DbContext genericContext) => RecipientLogic.PopulateDynamicRelations(genericContext, this);
        [NotMapped] public string MeasureName { get => Measure?.Name ?? string.Empty; }
        [NotMapped] public string RecipientTypeItem { get; set; } = string.Empty;
        [NotMapped] public string RecipientItem { get; set; } = string.Empty;
        [NotMapped] public string MeasureUnitName { get => Measure?.Unit?.Name ?? string.Empty; }
        [NotMapped] public string MeasurementUnitName { get => MeasurementUnit?.Name ?? string.Empty; }
    }
}
