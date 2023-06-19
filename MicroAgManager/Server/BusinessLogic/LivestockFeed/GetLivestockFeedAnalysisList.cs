using BackEnd.Abstracts;
using BackEnd.BusinessLogic.LivestockFeedAnalysis;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.LivestockFeed
{
    public class GetLivestockFeedAnalysisList : LivestockFeedAnalysisQueries, IRequest<Tuple<long, ICollection<LivestockFeedAnalysisModel?>>>
    {
        public class Handler : BaseRequestHandler<GetLivestockFeedAnalysisList>, IRequestHandler<GetLivestockFeedAnalysisList, Tuple<long, ICollection<LivestockFeedAnalysisModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<LivestockFeedAnalysisModel?>>> Handle(GetLivestockFeedAnalysisList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.LivestockFeedAnalysis>(_context);
                return new Tuple<long, ICollection<LivestockFeedAnalysisModel?>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => LivestockFeedAnalysisModel.Create(f)).ToListAsync(cancellationToken));
            }
        }
    }
}
