using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.ManyToMany
{
    public class GetLandPlotLivestockList : BaseQuery, IRequest<Tuple<long, ICollection<LandPlotLivestock?>>>
    {
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            throw new NotImplementedException();
        }

        private IQueryable<LandPlotLivestock> GetLandPlotLivestocks(IMicroAgManagementDbContext context)
        {
            var query = context.Plots.Where(m => m.TenantId == TenantId)
                .SelectMany(d => d.Livestocks.OrderByDescending(d => d.ModifiedOn).Select(s => new LandPlotLivestock(d.Id, s.Id,s.ModifiedOn)));


            if (Skip.HasValue || Take.HasValue)
                query = query.Skip(Skip ?? 0).Take(Take ?? 1000);
            query = query.OrderByDescending(_ => _.ModifiedOn);
            if (query is null) throw new ArgumentNullException(nameof(query));
            return query;
        }

        public class Handler : BaseRequestHandler<GetLandPlotLivestockList>, IRequestHandler<GetLandPlotLivestockList, Tuple<long, ICollection<LandPlotLivestock?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<LandPlotLivestock?>>> Handle(GetLandPlotLivestockList request, CancellationToken cancellationToken)
            {
                var query = request.GetLandPlotLivestocks(_context);
                return new Tuple<long, ICollection<LandPlotLivestock?>>
                    (await query.LongCountAsync(cancellationToken),
                    await query.ToListAsync(cancellationToken));
            }
        }
    }
}