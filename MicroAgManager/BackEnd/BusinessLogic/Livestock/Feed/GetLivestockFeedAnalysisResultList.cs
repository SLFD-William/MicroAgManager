using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Feed
{
    public class GetLivestockFeedAnalysisResultList : LivestockFeedAnalysisResultQueries, IRequest<Tuple<long, ICollection<LivestockFeedAnalysisResultModel?>>>
    {
        public class Handler : BaseRequestHandler<GetLivestockFeedAnalysisResultList>, IRequestHandler<GetLivestockFeedAnalysisResultList, Tuple<long, ICollection<LivestockFeedAnalysisResultModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<LivestockFeedAnalysisResultModel?>>> Handle(GetLivestockFeedAnalysisResultList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.LivestockFeedAnalysisResult>(_context);
                return new Tuple<long, ICollection<LivestockFeedAnalysisResultModel?>>
                    (await query.LongCountAsync(cancellationToken),
                    await query.Select(f => LivestockFeedAnalysisResultModel.Create(f)).ToListAsync(cancellationToken));
            }
        }
    }
}
