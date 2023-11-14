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

        private IQueryable<DutyMilestone> GetDutyMilestones(IMicroAgManagementDbContext context,out long count)
        {
            

            var items = context.Milestones.Include(m => m.Duties).Where(m => m.TenantId == TenantId && m.Duties.Any())
            .SelectMany(m => m.Duties.Select(d => new DutyMilestone(d.Id, m.Id, new[] { d.ModifiedOn, m.ModifiedOn }.Max())))
            .ToList();
            count=items.Count;
             var query = items.OrderByDescending(_ => _.ModifiedOn).AsQueryable();

            if (Skip.HasValue || Take.HasValue)
                query = query.Skip(Skip ?? 0).Take(Take ?? 1000);

            return query;
        }

        public class Handler : BaseRequestHandler<GetDutyMilestoneList>, IRequestHandler<GetDutyMilestoneList, DutyMilestoneDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<DutyMilestoneDto> Handle(GetDutyMilestoneList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query =request.GetDutyMilestones(context,out var count);
                    var models = await Task.FromResult(query.ToList());
                    return new DutyMilestoneDto(count, models);
                }
            }
        }
    }
}
