using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Livestock.Feed
{
    public class LivestockFeedServingQueries : BaseQuery
    {
        public LivestockFeedServingModel? NewLivestockFeedServing { get => (LivestockFeedServingModel?)NewModel; set => NewModel = value; }

        public long? FeedId { get; set; }
        public long? StatusId { get; set; }
        public decimal? Serving { get; set; }
        public string? ServingFrequency { get; set; }
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.LivestockFeedServings.AsQueryable());
            if (query is null)
                throw new ArgumentNullException(nameof(query));


            if (FeedId.HasValue) query = query.Where(f => f.Feed.Id == FeedId);
            if (StatusId.HasValue) query = query.Where(f => f.Status.Id == StatusId);
            if (Serving.HasValue) query = query.Where(f => f.Serving == Serving);


            return (IQueryable<T>)query;
        }

    }
}
