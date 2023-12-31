using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Chore
{
    public class ChoreQueries:BaseQuery
    {
        public ChoreModel? NewChore { get => (ChoreModel?)NewModel; set => NewModel = value; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.Chores.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (!string.IsNullOrWhiteSpace(Name)) query = query.Where(_ => _.Name.Contains(Name));
            if (!string.IsNullOrWhiteSpace(Color)) query = query.Where(_ => _.Color.Contains(Color));

            return (IQueryable<T>)query;
        }
    }
}
