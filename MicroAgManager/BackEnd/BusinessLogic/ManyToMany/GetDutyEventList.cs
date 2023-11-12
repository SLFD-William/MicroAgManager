using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.ManyToMany
{
    public class GetDutyEventList : BaseQuery, IRequest<Tuple<long, ICollection<DutyEvent?>>>
    {
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            throw new NotImplementedException();
        }

        private IQueryable<DutyEvent> GetDutyEvents(IMicroAgManagementDbContext context)
        {
            var query = context.Events.Where(m => m.TenantId == TenantId)
                .SelectMany(d => d.Duties.OrderByDescending(d => d.ModifiedOn).Select(s => new DutyEvent(s.Id, d.Id,d.ModifiedOn)));


            if (Skip.HasValue || Take.HasValue)
                query = query.Skip(Skip ?? 0).Take(Take ?? 1000);
            query = query.OrderByDescending(_ => _.ModifiedOn);
            if (query is null) throw new ArgumentNullException(nameof(query));
            return query;
        }

        public class Handler : BaseRequestHandler<GetDutyEventList>, IRequestHandler<GetDutyEventList, Tuple<long, ICollection<DutyEvent?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<DutyEvent?>>> Handle(GetDutyEventList request, CancellationToken cancellationToken)
            {
                var query = request.GetDutyEvents(_context);
                return new Tuple<long, ICollection<DutyEvent?>>
                    (await query.LongCountAsync(cancellationToken),
                    await query.ToListAsync(cancellationToken));
            }
        }
    }
}