using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Feed
{
    public class GetLivestockFeedDistributionList : LivestockFeedDistributionQueries, IRequest<Tuple<long, ICollection<LivestockFeedDistributionModel?>>>
    {
        public class Handler : BaseRequestHandler<GetLivestockFeedDistributionList>, IRequestHandler<GetLivestockFeedDistributionList, Tuple<long, ICollection<LivestockFeedDistributionModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<LivestockFeedDistributionModel?>>> Handle(GetLivestockFeedDistributionList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.LivestockFeedDistribution>(_context);
                return new Tuple<long, ICollection<LivestockFeedDistributionModel?>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => LivestockFeedDistributionModel.Create(f)).ToListAsync(cancellationToken));
            }
        }
    }
}
