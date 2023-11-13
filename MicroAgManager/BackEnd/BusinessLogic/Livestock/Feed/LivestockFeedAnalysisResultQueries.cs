using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Livestock.Feed
{
    public class LivestockFeedAnalysisResultQueries : BaseQuery
    {
        public LivestockFeedAnalysisResultModel? NewLivestockFeedAnalysisResult { get => (LivestockFeedAnalysisResultModel?)NewModel; set => NewModel = value; }
        public long? AnalysisId { get; set; }
        public long? ParameterId { get; set; }
        public decimal? AsFed { get; set; }
        public decimal? Dry { get; set; }
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.LivestockFeedAnalysisResults.AsQueryable());
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            if (AnalysisId.HasValue) query = query.Where(x => x.Analysis.Id == AnalysisId);
            if (ParameterId.HasValue) query = query.Where(x => x.Parameter.Id == ParameterId);
            if (AsFed.HasValue) query = query.Where(x => x.AsFed == AsFed);
            if (Dry.HasValue) query = query.Where(x => x.Dry == Dry);

            return (IQueryable<T>)query;
        }

    }
}
