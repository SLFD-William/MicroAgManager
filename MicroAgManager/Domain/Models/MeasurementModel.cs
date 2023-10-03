﻿using Domain.Abstracts;
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
        public Measurement MapToEntity(Measurement measurement)
        {
            measurement.MeasureId = MeasureId;
            measurement.RecipientTypeId = RecipientTypeId;
            measurement.RecipientType = RecipientType;
            measurement.RecipientId = RecipientId;
            measurement.Value = Value;
            measurement.MeasurementUnitId = MeasurementUnitId;
            measurement.Notes = Notes;
            measurement.DatePerformed = DatePerformed;
            measurement.ModifiedOn = DateTime.UtcNow;
            return measurement;
        }
    }
}
