using Domain.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    public class MeasurementModel : BaseModel
    {
        [Required][ForeignKey("Measure")] public long MeasureId { get; set; }
        public virtual MeasureModel Measure { get; set; }
        [Required] public long RecipientTypeId { get; set; }
        [Required][MaxLength(40)] public string RecipientType { get; set; }
        [Required] public long RecipientId { get; set; }
        [Precision(18, 3)][Required] public decimal Value { get; set; }
        [Required][ForeignKey("MeasurementUnit")] public long MeasurementUnitId { get; set; }
        public virtual UnitModel MeasurementUnit { get; set; }
        public string Notes { get; set; }
        [Required] public DateTime DatePerformed { get; set; }

        public static MeasurementModel Create(Measurement measurement)
        {
            var model = PopulateBaseModel(measurement, new MeasurementModel
            {
                MeasureId = measurement.MeasureId,
                RecipientTypeId = measurement.RecipientTypeId,
                RecipientType = measurement.RecipientType,
                RecipientId = measurement.RecipientId,
                Value = measurement.Value,
                MeasurementUnitId = measurement.MeasurementUnitId,
                Notes = measurement.Notes,
                DatePerformed = measurement.DatePerformed
            }) as MeasurementModel;
            return model;
        }

        public override BaseModel Map(BaseModel measurement)
        {
            if (measurement == null || measurement is not MeasurementModel) return null;
            ((MeasurementModel) measurement).MeasureId = MeasureId;
            ((MeasurementModel)measurement).RecipientTypeId = RecipientTypeId;
            ((MeasurementModel)measurement).RecipientType = RecipientType;
            ((MeasurementModel)measurement).RecipientId = RecipientId;
            ((MeasurementModel)measurement).Value = Value;
            ((MeasurementModel)measurement).MeasurementUnitId = MeasurementUnitId;
            ((MeasurementModel)measurement).Notes = Notes;
            ((MeasurementModel)measurement).DatePerformed = DatePerformed;
            return measurement;
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
    }
}
