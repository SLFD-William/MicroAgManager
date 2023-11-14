using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Feed
{
    public class GetLivestockFeedAnalysisList : LivestockFeedAnalysisQueries, IRequest<Tuple<long, ICollection<LivestockFeedAnalysisModel?>>>
    {
        public class Handler : BaseRequestHandler<GetLivestockFeedAnalysisList>, IRequestHandler<GetLivestockFeedAnalysisList, Tuple<long, ICollection<LivestockFeedAnalysisModel?>>>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<LivestockFeedAnalysisModel?>>> Handle(GetLivestockFeedAnalysisList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.LivestockFeedAnalysis>(context);
                    return new Tuple<long, ICollection<LivestockFeedAnalysisModel?>>(await query.LongCountAsync(cancellationToken), await query.Select(f => LivestockFeedAnalysisModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
