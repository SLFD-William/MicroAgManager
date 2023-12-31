using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.ManyToMany
{
    public class GetDutyChoreList : BaseQuery, IRequest<DutyChoreDto>
    {
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            throw new NotImplementedException();
        }

        private IQueryable<DutyChore> GetDutyChores(IMicroAgManagementDbContext context,out long count)
        {
            var items =context.Chores.Include(d => d.Duties).Where(m => m.TenantId == TenantId && m.Duties.Any())
                .SelectMany(e => e.Duties.Select(d => new DutyChore(d.Id, e.Id, new[] { d.ModifiedOn, e.ModifiedOn }.Max())))
                .ToList();
            count=items.Count;
            var query = items.OrderByDescending(_ => _.ModifiedOn).AsQueryable();
            if (Skip.HasValue || Take.HasValue)
                query = query.Skip(Skip ?? 0).Take(Take ?? 1000);
            return query;
        }

        public class Handler : BaseRequestHandler<GetDutyChoreList>, IRequestHandler<GetDutyChoreList, DutyChoreDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<DutyChoreDto> Handle(GetDutyChoreList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query =request.GetDutyChores(context, out var count);
                    var models =await Task.FromResult( query.ToList());
                    return new DutyChoreDto(count, models);
                }
            }
        }
    }
}