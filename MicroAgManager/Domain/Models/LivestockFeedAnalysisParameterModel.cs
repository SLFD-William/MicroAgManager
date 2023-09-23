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
        public LivestockFeedAnalysisParameter MapToEntity(LivestockFeedAnalysisParameter entity)
        {
            entity.Parameter = Parameter;
            entity.SubParameter = SubParameter;
            entity.Unit = Unit;
            entity.Method = Method;
            entity.ModifiedOn = DateTime.UtcNow;
            return entity;
        }
    }
}
