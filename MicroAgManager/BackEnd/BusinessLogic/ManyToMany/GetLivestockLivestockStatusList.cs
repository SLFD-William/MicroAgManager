using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.ManyToMany
{
    public class GetLivestockLivestockStatusList : BaseQuery, IRequest<Tuple<long, ICollection<LivestockLivestockStatus?>>>
    {
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            throw new NotImplementedException();
        }

        private IQueryable<LivestockLivestockStatus> GetLivestockLivestockStatuss(IMicroAgManagementDbContext context)
        {
            var query = context.LivestockStatuses.Where(m => m.TenantId == TenantId)
                .SelectMany(d => d.Livestocks.OrderByDescending(d => d.ModifiedOn).Select(s => new LivestockLivestockStatus(d.Id, s.Id)));


            if (Skip.HasValue || Take.HasValue)
                query = query.Skip(Skip ?? 0).Take(Take ?? 1000);

            if (query is null) throw new ArgumentNullException(nameof(query));
            return query;
        }

        public class Handler : BaseRequestHandler<GetLivestockLivestockStatusList>, IRequestHandler<GetLivestockLivestockStatusList, Tuple<long, ICollection<LivestockLivestockStatus?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<LivestockLivestockStatus?>>> Handle(GetLivestockLivestockStatusList request, CancellationToken cancellationToken)
            {
                var query = request.GetLivestockLivestockStatuss(_context);
                return new Tuple<long, ICollection<LivestockLivestockStatus?>>
                    (await query.LongCountAsync(cancellationToken),
                    await query.ToListAsync(cancellationToken));
            }
        }
    }
}