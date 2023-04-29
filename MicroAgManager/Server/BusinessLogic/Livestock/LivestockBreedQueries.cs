using BackEnd.Abstracts;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Livestock
{
    public class LivestockBreedQueries : BaseQuery
    {
        public LivestockBreedModel? NewBreed { get => (LivestockBreedModel?)NewModel; set => NewModel = value; }
        public long? LivestockTypeId { get; set; }
        public string? Name { get; set; }
        public string? EmojiChar { get; set; }
        public int? GestationPeriod { get; set; }
        public int? HeatPeriod { get; set; }
        public IQueryable<LivestockBreed> GetQuery(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.LivestockBreeds.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));
            
            if(LivestockTypeId.HasValue) query = query.Where(_ => _.Livestock.Id == LivestockTypeId);
            if (!string.IsNullOrEmpty(Name)) query = query.Where(_ => _.Name != null && _.Name.Contains(Name));
            if (!string.IsNullOrEmpty(EmojiChar)) query = query.Where(_ => _.EmojiChar != null && _.EmojiChar.Contains(EmojiChar));
            if (GestationPeriod.HasValue) query = query.Where(_ => _.GestationPeriod==GestationPeriod);
            if (HeatPeriod.HasValue) query = query.Where(_ => _.HeatPeriod == HeatPeriod);
            
            query = query.OrderByDescending(_ => _.ModifiedOn);
            return query;
        }
    }
}
