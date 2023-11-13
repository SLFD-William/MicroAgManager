using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.ScheduledDuty
{
    public class ScheduledDutyQueries:BaseQuery
    {
        public ScheduledDutyModel? NewDuty { get => (ScheduledDutyModel?)NewModel; set => NewModel = value; }
        public DateTime? DueOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public Guid? CompletedBy { get; set; }
        public bool? Dismissed { get; set; }
        public long? DutyId { get; set; }
        public decimal? ReminderDays { get; set; }
        

        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.ScheduledDuties.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (DueOn.HasValue) query = query.Where(_ => _.DueOn == DueOn);
            if (CompletedOn.HasValue) query = query.Where(_ => _.CompletedOn == CompletedOn);
            if (CompletedBy.HasValue) query = query.Where(_ => _.CompletedBy == CompletedBy);
            if (Dismissed.HasValue) query = query.Where(_ => _.Dismissed == Dismissed);
            if (DutyId.HasValue) query = query.Where(_ => _.Duty.Id == DutyId);
            if (ReminderDays.HasValue) query = query.Where(_ => _.ReminderDays == ReminderDays);
            return (IQueryable<T>)query;
        }
    }
}
