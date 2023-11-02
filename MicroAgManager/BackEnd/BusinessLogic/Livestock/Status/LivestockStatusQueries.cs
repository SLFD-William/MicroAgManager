using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Livestock.Status
{
    public class LivestockStatusQueries : BaseQuery
    {
        public long? LivestockAnimalId { get; set; }
        public string? Status { get; set; }

        public LivestockStatusModel? NewLivestockStatus { get => (LivestockStatusModel?)NewModel; set => NewModel = value; }
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.LivestockStatuses.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));

            if (LivestockAnimalId.HasValue) query = query.Where(l => l.LivestockAnimal.Id == LivestockAnimalId);
            if (!string.IsNullOrEmpty(Status)) query = query.Where(l => l.Status.Contains(Status));
            query = query.OrderByDescending(_ => _.ModifiedOn);

            return (IQueryable<T>)query;
        }
    }
}
