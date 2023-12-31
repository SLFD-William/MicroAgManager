using Domain.Abstracts;
using Domain.Constants;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class MeasureModel : BaseModel
    {
        [Required][MaxLength(20)] public string Method { get; set; } = nameof(MeasurementMethodConstants.Direct);
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required][ForeignKey("Unit")] public long UnitId { get; set; }
        public virtual UnitModel Unit { get; set; }

        public static MeasureModel Create(Measure measure)
        {
            var model = PopulateBaseModel(measure, new MeasureModel
            {
                UnitId = measure.UnitId,
                Method = measure.Method,
                Name = measure.Name
            }) as MeasureModel;
            return model;
        }
        public override BaseModel Map(BaseModel measure)
        {
            if (measure == null || measure is not MeasureModel) return null;
           ((MeasureModel) measure).UnitId = UnitId;
            ((MeasureModel)measure).Method = Method;
            ((MeasureModel)measure).Name = Name;
            ((MeasureModel)measure).EntityModifiedOn = EntityModifiedOn;
            return measure;
        }

        public override BaseEntity Map(BaseEntity measure)
        {
            if (measure == null || measure is not Measure) return null;
            ((Measure)measure).UnitId = UnitId;
            ((Measure)measure).Method = Method;
            ((Measure)measure).Name = Name;
            ((Measure)measure).ModifiedOn = DateTime.UtcNow;
            return measure;
        }
    }
}
