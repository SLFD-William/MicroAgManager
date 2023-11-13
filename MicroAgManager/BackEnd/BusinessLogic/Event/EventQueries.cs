using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Event
{
    public class EventQueries:BaseQuery
    {
        public EventModel? NewEvent { get => (EventModel?)NewModel; set => NewModel = value; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.Events.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (StartDate.HasValue) query = query.Where(_ => _.StartDate >= StartDate);
            if (EndDate.HasValue) query = query.Where(_ => _.EndDate <= EndDate);
            if (!string.IsNullOrWhiteSpace(Name)) query = query.Where(_ => _.Name.Contains(Name));
            if (!string.IsNullOrWhiteSpace(Color)) query = query.Where(_ => _.Color.Contains(Color));

            return (IQueryable<T>)query;
        }
    }
}
