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

        private IQueryable<DutyEvent> GetDutyEvents(IMicroAgManagementDbContext context)
        {
            var query = context.Events.Include(d=>d.Duties).Where(m => m.TenantId == TenantId)
                .SelectMany(e => e.Duties.Select(d => new DutyEvent(d.Id, e.Id, new[] { d.ModifiedOn, e.ModifiedOn }.Max())))
                .OrderByDescending(_ => _.ModifiedOn).AsQueryable();


            if (Skip.HasValue || Take.HasValue)
                query = query.Skip(Skip ?? 0).Take(Take ?? 1000);

            if (query is null) throw new ArgumentNullException(nameof(query));
            return query;
        }

        public class Handler : BaseRequestHandler<GetDutyEventList>, IRequestHandler<GetDutyEventList, DutyEventDto>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<DutyEventDto> Handle(GetDutyEventList request, CancellationToken cancellationToken)
            {
                var query = request.GetDutyEvents(_context);
                try
                {
                    var count = await query.LongCountAsync(cancellationToken);
                    var models = await query.ToListAsync(cancellationToken);
                    return new DutyEventDto(count, models);
                }
                catch
                {
                    return new DutyEventDto(0, new List<DutyEvent>());
                }
            }
        }
    }
}