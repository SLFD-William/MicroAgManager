using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    public class LivestockFeedAnalysisResultModel : BaseModel
    {
        [ForeignKey(nameof(LivestockFeedAnalysisModel))]
        [Required] public long AnalysisId { get; set; }
        [ForeignKey(nameof(LivestockFeedAnalysisParameterModel))]
        [Required] public long ParameterId { get; set; }

        [Required][Precision(18,2)][Range(0, (double)decimal.MaxValue)] public decimal AsFed { get; set; }
        [Required][Precision(18, 2)][Range(0, (double)decimal.MaxValue)] public decimal Dry { get; set; }
        public static LivestockFeedAnalysisResultModel? Create(LivestockFeedAnalysisResult livestockBreed)
        {
            var model = PopulateBaseModel(livestockBreed, new LivestockFeedAnalysisResultModel
            {
                AnalysisId=livestockBreed.Analysis.Id,
                ParameterId=livestockBreed.Parameter.Id,
                AsFed=livestockBreed.AsFed,
                Dry=livestockBreed.Dry
            }) as LivestockFeedAnalysisResultModel;
            return model;
        }
        public override BaseModel Map(BaseModel entity)
        {
            if (entity == null || entity is not LivestockFeedAnalysisResultModel) return null;
            ((LivestockFeedAnalysisResultModel)entity).ParameterId = ParameterId;
            ((LivestockFeedAnalysisResultModel)entity).AsFed = AsFed;
            ((LivestockFeedAnalysisResultModel)entity).Dry = Dry;
            ((LivestockFeedAnalysisResultModel)entity).AnalysisId = AnalysisId;
            return entity;
        }

        public override BaseEntity Map(BaseEntity entity)
        {
            if (entity == null || entity is not LivestockFeedAnalysisResult) return null;
            ((LivestockFeedAnalysisResult)entity).ParameterId = ParameterId;
            ((LivestockFeedAnalysisResult)entity).AsFed = AsFed;
            ((LivestockFeedAnalysisResult)entity).Dry = Dry;
            ((LivestockFeedAnalysisResult)entity).AnalysisId = AnalysisId;
            entity.ModifiedOn = DateTime.UtcNow;
            return entity;
        }
    }
}