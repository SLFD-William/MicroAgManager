using BackEnd.Abstracts;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Feed
{
    public class GetLivestockFeedAnalysisParameterList : LivestockFeedAnalysisParameterQueries, IRequest<Tuple<long, ICollection<LivestockFeedAnalysisParameterModel?>>>
    {
        public class Handler : BaseRequestHandler<GetLivestockFeedAnalysisParameterList>, IRequestHandler<GetLivestockFeedAnalysisParameterList, Tuple<long, ICollection<LivestockFeedAnalysisParameterModel?>>>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<LivestockFeedAnalysisParameterModel?>>> Handle(GetLivestockFeedAnalysisParameterList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.LivestockFeedAnalysisParameter>(context);
                    return new Tuple<long, ICollection<LivestockFeedAnalysisParameterModel?>>(await query.LongCountAsync(cancellationToken), await query.Select(f => LivestockFeedAnalysisParameterModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
