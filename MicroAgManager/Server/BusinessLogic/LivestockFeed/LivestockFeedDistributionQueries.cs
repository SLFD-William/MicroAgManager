using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.LivestockFeed
{
    public class LivestockFeedDistributionQueries : BaseQuery
    {
        public LivestockFeedDistributionModel? NewLivestockFeedDistribution { get => (LivestockFeedDistributionModel?)NewModel; set => NewModel = value; }
        public long? FeedId { get; set; }
        public decimal? Quantity { get; set; }
        public bool? Discarded { get; set; }
        public string? Note { get; set; }
        public DateTime? DatePerformed { get; set; }
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.LivestockFeedDistributions.AsQueryable());
            if (query is null)
                throw new ArgumentNullException(nameof(query));
            if (FeedId.HasValue) query = query.Where(x => x.Feed.Id == FeedId);
            if (Quantity.HasValue) query = query.Where(x => x.Quantity == Quantity);
            if (Discarded.HasValue) query = query.Where(x => x.Discarded == Discarded);
            if (!string.IsNullOrWhiteSpace(Note)) query = query.Where(x => x.Note.Contains(Note));
            if (DatePerformed.HasValue) query = query.Where(x => x.DatePerformed == DatePerformed);

            query = query.OrderByDescending(_ => _.ModifiedOn);
            return (IQueryable<T>)query;
        }

    }
}
