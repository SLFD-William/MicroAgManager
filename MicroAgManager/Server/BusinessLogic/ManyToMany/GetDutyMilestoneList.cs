using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.ManyToMany
{
    public class GetDutyMilestoneList: BaseQuery, IRequest<Tuple<long, ICollection<DutyMilestone?>>>
    {
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            throw new NotImplementedException();
        }

        private IQueryable<DutyMilestone> GetDutyMilestones(IMicroAgManagementDbContext context)
        {
            var query = context.Milestones.Where(m => m.TenantId == TenantId)
                .SelectMany(d => d.Duties.OrderByDescending(d => d.ModifiedOn).Select(s => new DutyMilestone(s.Id, d.Id)));
        

            if (Skip.HasValue || Take.HasValue)
                query = query.Skip(Skip ?? 0).Take(Take ?? 1000);

            if (query is null) throw new ArgumentNullException(nameof(query));
            return query;
        }

        public class Handler : BaseRequestHandler<GetDutyMilestoneList>, IRequestHandler<GetDutyMilestoneList, Tuple<long, ICollection<DutyMilestone?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<DutyMilestone?>>> Handle(GetDutyMilestoneList request, CancellationToken cancellationToken)
            {
                var query = request.GetDutyMilestones(_context);
                return new Tuple<long, ICollection<DutyMilestone?>>
                    (await query.LongCountAsync(cancellationToken),
                    await query.ToListAsync(cancellationToken));
            }
        }
    }
}
