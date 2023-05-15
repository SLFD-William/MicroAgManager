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
        public LivestockFeedAnalysis MapToEntity(LivestockFeedAnalysis entity)
        {
            entity.LabNumber = LabNumber;
            entity.DateReceived = DateReceived;
            entity.DateReported = DateReported;
            entity.DateSampled = DateSampled;
            entity.TestCode = TestCode;
            entity.DatePrinted = DatePrinted;
            entity.LivestockFeedId=FeedId;
            if (entity.Results?.Any() ?? false)
                foreach (var breed in entity.Results)
                    Results?.FirstOrDefault(p => p?.Id == breed.Id)?.MapToEntity(breed);
            return entity;
        }
    }
}
