using Domain.Abstracts;
using Domain.Constants;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class MeasureModel : BaseModel
    {
        [Required][MaxLength(20)] public string Unit { get; set; } = nameof(MeasurementUnitConstants.Weight_Pounds);
        [Required][MaxLength(20)] public string MeasurementType { get; set; } = nameof(MeasurementTypeConstants.Weight);
        [Required][MaxLength(20)] public string Method { get; set; } = nameof(MeasurementMethodConstants.Direct);
        [Required][MaxLength(40)] public string Name { get; set; }

        public static MeasureModel Create(Measure measure)
        {
            var model = PopulateBaseModel(measure, new MeasureModel
            {
                Unit = measure.Unit,
                MeasurementType = measure.MeasurementType,
                Method = measure.Method,
                Name = measure.Name
            }) as MeasureModel;
            return model;
        }
        public Measure MapToEntity(Measure measure)
        {
            measure.Unit = Unit;
            measure.MeasurementType = MeasurementType;
            measure.Method = Method;
            measure.Name = Name;
            measure.ModifiedOn = DateTime.UtcNow;
            return measure;
        }
    }
}
