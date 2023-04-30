using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.LivestockFeedAnalysis
{
    public class LivestockFeedAnalysisQueries : BaseQuery
    {
        public LivestockFeedAnalysisModel? NewLivestockFeedAnalysis { get => (LivestockFeedAnalysisModel?)NewModel; set => NewModel = value; }
        public string? LabNumber { get; set; }
        public long? FeedId { get; set; }
        public string? TestCode { get; set; }
        public DateTime? DateSampled { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateReported { get; set; }
        public DateTime? DatePrinted { get; set; }
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.LivestockFeedAnalyses.AsQueryable());
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            if (!string.IsNullOrWhiteSpace(LabNumber)) query = query.Where(x => x.LabNumber.Contains(LabNumber));
            if (FeedId.HasValue) query = query.Where(x => x.Feed.Id == FeedId);
            if (!string.IsNullOrWhiteSpace(TestCode)) query = query.Where(x => x.TestCode.Contains(TestCode));
            if (DateSampled.HasValue) query = query.Where(x => x.DateSampled == DateSampled);
            if (DateReceived.HasValue) query = query.Where(x => x.DateReceived == DateReceived);
            if (DateReported.HasValue) query = query.Where(x => x.DateReported == DateReported);
            if (DatePrinted.HasValue) query = query.Where(x => x.DatePrinted == DatePrinted);

            query = query.OrderByDescending(_ => _.ModifiedOn);
            return (IQueryable<T>)query;
        }

    }
}
