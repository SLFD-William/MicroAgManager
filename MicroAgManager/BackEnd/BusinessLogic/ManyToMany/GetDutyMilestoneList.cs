using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.ManyToMany
{
    public class GetDutyMilestoneList: BaseQuery, IRequest<DutyMilestoneDto>
    {
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            throw new NotImplementedException();
        }

        private IQueryable<DutyMilestone> GetDutyMilestones(IMicroAgManagementDbContext context)
        {
            var query = context.Milestones.Include(m => m.Duties).Where(m => m.TenantId == TenantId)
            .SelectMany(m => m.Duties.Select(d => new DutyMilestone(d.Id, m.Id, new[] { d.ModifiedOn, m.ModifiedOn }.Max())))
            .OrderByDescending(_ => _.ModifiedOn).AsQueryable();

            if (Skip.HasValue || Take.HasValue)
                query = query.Skip(Skip ?? 0).Take(Take ?? 1000);

            return query;
        }

        public class Handler : BaseRequestHandler<GetDutyMilestoneList>, IRequestHandler<GetDutyMilestoneList, DutyMilestoneDto>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<DutyMilestoneDto> Handle(GetDutyMilestoneList request, CancellationToken cancellationToken)
            {
                var query = request.GetDutyMilestones(_context);
                try
                { 
                    var count = await query.LongCountAsync(cancellationToken);
                    var models= await query.ToListAsync(cancellationToken);
                    return new DutyMilestoneDto(count,models);
                }
                catch { 
                    return new DutyMilestoneDto(0, new List<DutyMilestone>() );
                }
            }
        }
    }
}
