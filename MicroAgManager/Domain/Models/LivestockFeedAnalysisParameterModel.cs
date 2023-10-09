using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class LivestockFeedAnalysisParameterModel:BaseModel
    {
        [Required][MaxLength(50)] public string Parameter { get; set; }
        [Required][MaxLength(50)] public string SubParameter { get; set; }
        [Required][MaxLength(50)] public string Unit { get; set; }
        [Required][MaxLength(50)] public string Method { get; set; }
        [Required][Range(1, int.MaxValue)] public int ReportOrder { get; set; }
        public static LivestockFeedAnalysisParameterModel? Create(LivestockFeedAnalysisParameter livestockBreed)
        {
            var model = PopulateBaseModel(livestockBreed, new LivestockFeedAnalysisParameterModel
            {
                Parameter = livestockBreed.Parameter,
                SubParameter = livestockBreed.SubParameter,
                Unit = livestockBreed.Unit,
                Method = livestockBreed.Method
            }) as LivestockFeedAnalysisParameterModel;
            return model;
        }

        public override BaseModel Map(BaseModel entity)
        {
            if (entity == null || entity is not LivestockFeedAnalysisParameterModel) return null;
            ((LivestockFeedAnalysisParameterModel)entity).Parameter = Parameter;
            ((LivestockFeedAnalysisParameterModel)entity).SubParameter = SubParameter;
            ((LivestockFeedAnalysisParameterModel)entity).Unit = Unit;
            ((LivestockFeedAnalysisParameterModel)entity).Method = Method;
            return entity;
        }

        public override BaseEntity Map(BaseEntity entity)
        {
            if (entity == null || entity is not LivestockFeedAnalysisParameter) return null;
            ((LivestockFeedAnalysisParameter)entity).Parameter = Parameter;
            ((LivestockFeedAnalysisParameter)entity).SubParameter = SubParameter;
            ((LivestockFeedAnalysisParameter)entity).Unit = Unit;
            ((LivestockFeedAnalysisParameter)entity).Method = Method;
            entity.ModifiedOn = DateTime.UtcNow;
            return entity;
        }
    }
}
