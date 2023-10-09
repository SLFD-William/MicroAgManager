using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class LivestockFeedAnalysisModel : BaseModel
    {
        [MaxLength(40)]public string LabNumber { get; set; }

        [Required]
        [ForeignKey(nameof(LivestockFeedModel))]
        public long FeedId { get; set; }
        [MaxLength(40)] public string TestCode { get; set; }
        public DateTime? DateSampled { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateReported { get; set; }
        public DateTime? DatePrinted { get; set; }
        public ICollection<LivestockFeedAnalysisResultModel?> Results { get; set; } = new List<LivestockFeedAnalysisResultModel?>();
        public static LivestockFeedAnalysisModel? Create(LivestockFeedAnalysis feedAnalysis)
        {
            var model = PopulateBaseModel(feedAnalysis, new LivestockFeedAnalysisModel
            {
                LabNumber = feedAnalysis.LabNumber,
                FeedId = feedAnalysis.LivestockFeedId,
                DatePrinted = feedAnalysis.DatePrinted,
                DateReceived = feedAnalysis.DateReceived,
                DateReported = feedAnalysis.DateReported,
                DateSampled = feedAnalysis.DateSampled,
                TestCode = feedAnalysis.TestCode,
                Results = feedAnalysis.Results.Select(LivestockFeedAnalysisResultModel.Create).ToList() ?? new List<LivestockFeedAnalysisResultModel?>()
            }) as LivestockFeedAnalysisModel;
            return model;
        }
        public override BaseModel Map(BaseModel entity)
        {
            if (entity == null || entity is not LivestockFeedAnalysisModel) return null;
            ((LivestockFeedAnalysisModel)entity).LabNumber = LabNumber;
            ((LivestockFeedAnalysisModel)entity).DateReceived = DateReceived;
            ((LivestockFeedAnalysisModel)entity).DateReported = DateReported;
            ((LivestockFeedAnalysisModel)entity).DateSampled = DateSampled;
            ((LivestockFeedAnalysisModel)entity).TestCode = TestCode;
            ((LivestockFeedAnalysisModel)entity).DatePrinted = DatePrinted;
            ((LivestockFeedAnalysisModel)entity).FeedId = FeedId;
            if (((LivestockFeedAnalysisModel)entity).Results?.Any() ?? false)
                foreach (var breed in ((LivestockFeedAnalysisModel)entity).Results)
                    Results?.FirstOrDefault(p => p?.Id == breed.Id)?.Map(breed);
            return entity;
        }

        public override BaseEntity Map(BaseEntity entity)
        {
            if (entity == null || entity is not LivestockFeedAnalysis) return null;
            ((LivestockFeedAnalysis)entity).LabNumber = LabNumber;
            ((LivestockFeedAnalysis)entity).DateReceived = DateReceived;
            ((LivestockFeedAnalysis)entity).DateReported = DateReported;
            ((LivestockFeedAnalysis)entity).DateSampled = DateSampled;
            ((LivestockFeedAnalysis)entity).TestCode = TestCode;
            ((LivestockFeedAnalysis)entity).DatePrinted = DatePrinted;
            ((LivestockFeedAnalysis)entity).LivestockFeedId = FeedId;
            entity.ModifiedOn = DateTime.UtcNow;
            if (((LivestockFeedAnalysis)entity).Results?.Any() ?? false)
                foreach (var breed in ((LivestockFeedAnalysis)entity).Results)
                    Results?.FirstOrDefault(p => p?.Id == breed.Id)?.Map(breed);
            return entity;
        }
    }
}
