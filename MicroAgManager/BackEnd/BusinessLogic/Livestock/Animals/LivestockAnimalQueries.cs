using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Livestock.Animals
{
    public class LivestockAnimalQueries : BaseQuery
    {
        public LivestockAnimalModel? NewLivestockAnimal { get => (LivestockAnimalModel?)NewModel; set => NewModel = value; }

        public string? Name { get; set; }
        public string? GroupName { get; set; }
        public string? ParentMaleName { get; set; }
        public string? ParentFemaleName { get; set; }
        public string? Care { get; set; }

        public string? Breed { get; set; }
        public string? Status { get; set; }
        public string? Feed { get; set; }

        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.LivestockAnimals.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));

            if (!string.IsNullOrEmpty(Name)) query = query.Where(_ => _.Name != null && _.Name.Contains(Name));
            if (!string.IsNullOrEmpty(GroupName)) query = query.Where(_ => _.GroupName != null && _.Name.Contains(GroupName));
            if (!string.IsNullOrEmpty(ParentMaleName)) query = query.Where(_ => _.ParentMaleName != null && _.Name.Contains(ParentMaleName));
            if (!string.IsNullOrEmpty(ParentFemaleName)) query = query.Where(_ => _.ParentFemaleName != null && _.Name.Contains(ParentFemaleName));
            if (!string.IsNullOrEmpty(Care)) query = query.Where(_ => _.Care != null && _.Name.Contains(Care));
            if (!string.IsNullOrEmpty(Breed)) query = query.Where(_ => _.Breeds.Any(b => b.Name.Contains(Breed)));
            if (!string.IsNullOrEmpty(Status)) query = query.Where(_ => _.Statuses.Any(b => b.Status.Contains(Status)));
            if (!string.IsNullOrEmpty(Feed)) query = query.Where(_ => _.Feeds.Any(b => b.Name.Contains(Feed) || b.Source.Contains(Feed)));
            return (IQueryable<T>)query;
        }


    }
}
