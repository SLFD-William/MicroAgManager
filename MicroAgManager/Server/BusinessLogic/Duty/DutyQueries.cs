using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Duty
{
    public class DutyQueries:BaseQuery
    {
        public DutyModel? NewDuty { get => (DutyModel?)NewModel; set => NewModel = value; }
        public int? DaysDue { get; set; }
        public string? DutyType { get; set; }
        public long? CommandId { get; set; }
        public string? Relationship { get; set; }
        public string? Gender { get; set; }
        public bool? SystemRequired { get; set; }

        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.Duties.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (DaysDue.HasValue) query = query.Where(_ => _.DaysDue == DaysDue);
            if (!string.IsNullOrWhiteSpace(DutyType)) query = query.Where(_ => _.Command != null && _.Command.Contains(DutyType));
            if (CommandId.HasValue) query = query.Where(_ => _.CommandId == CommandId);
            if (!string.IsNullOrWhiteSpace(Relationship)) query = query.Where(_ => _.Relationship != null && _.Relationship.Contains(Relationship));
            if (!string.IsNullOrWhiteSpace(Gender)) query = query.Where(_ => _.Gender != null && _.Gender.Contains(Gender));
            if (SystemRequired.HasValue) query = query.Where(_ => _.SystemRequired == SystemRequired);
            return (IQueryable<T>)query;
        }
    }
}
