using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.ManyToMany
{
    public class GetDutyEventList : BaseQuery, IRequest<DutyEventDto>
    {
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            throw new NotImplementedException();
        }

        private IQueryable<DutyEvent> GetDutyEvents(IMicroAgManagementDbContext context,out long count)
        {
            var items =context.Events.Include(d => d.Duties).Where(m => m.TenantId == TenantId && m.Duties.Any())
                .SelectMany(e => e.Duties.Select(d => new DutyEvent(d.Id, e.Id, new[] { d.ModifiedOn, e.ModifiedOn }.Max())))
                .ToList();
            count=items.Count;
            var query = items.OrderByDescending(_ => _.ModifiedOn).AsQueryable();
            if (Skip.HasValue || Take.HasValue)
                query = query.Skip(Skip ?? 0).Take(Take ?? 1000);
            return query;
        }

        public class Handler : BaseRequestHandler<GetDutyEventList>, IRequestHandler<GetDutyEventList, DutyEventDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<DutyEventDto> Handle(GetDutyEventList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query =request.GetDutyEvents(context, out var count);
                    var models =await Task.FromResult( query.ToList());
                    return new DutyEventDto(count, models);
                }
            }
        }
    }
}