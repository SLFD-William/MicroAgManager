using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.LivestockFeed
{
    public class LivestockFeedQueries : BaseQuery
    {
        public LivestockFeedModel? NewLivestockFeed { get => (LivestockFeedModel?)NewModel; set => NewModel = value; }

        public long? LivestockTypeId { get; set; }
        public string? Name { get; set; }
        public string? Source { get; set; }
        public int? Cutting { get; set; }
        public bool? Active { get; set; }
        public decimal? Quantity { get; set; }
        public string? QuantityUnit { get; set; }
        public decimal? QuantityWarning { get; set; }
        public string? FeedType { get; set; }
        public string? Distribution { get; set; }

        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.LivestockFeeds.AsQueryable());
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            if (LivestockTypeId.HasValue) query = query.Where(x => x.LivestockType.Id == LivestockTypeId);
            if (!string.IsNullOrWhiteSpace(Name)) query = query.Where(x => x.Name.Contains(Name));
            if (!string.IsNullOrWhiteSpace(Source)) query = query.Where(x => x.Source.Contains(Source));
            if (Cutting.HasValue) query = query.Where(x => x.Cutting.HasValue && x.Cutting==Cutting);
            if (Active.HasValue) query = query.Where(x => x.Active.HasValue && x.Active == Active);
            if (Quantity.HasValue) query = query.Where(x => x.Quantity == Quantity);
            if (!string.IsNullOrWhiteSpace(QuantityUnit)) query = query.Where(x => x.QuantityUnit.Contains(QuantityUnit));
            if (QuantityWarning.HasValue) query = query.Where(x => x.QuantityWarning == QuantityWarning);
            if (!string.IsNullOrWhiteSpace(FeedType)) query = query.Where(x => x.FeedType.Contains(FeedType));
            if (!string.IsNullOrWhiteSpace(Distribution)) query = query.Where(x => x.Distribution.Contains(Distribution));

            query = query.OrderByDescending(_ => _.ModifiedOn);
            return (IQueryable<T>)query;
        }

    }
}
