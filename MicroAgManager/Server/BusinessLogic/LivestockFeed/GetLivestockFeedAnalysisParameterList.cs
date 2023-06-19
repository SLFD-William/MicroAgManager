using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.LivestockFeed
{
    public class GetLivestockFeedAnalysisParameterList : LivestockFeedAnalysisParameterQueries, IRequest<Tuple<long, ICollection<LivestockFeedAnalysisParameterModel?>>>
    {
        public class Handler : BaseRequestHandler<GetLivestockFeedAnalysisParameterList>, IRequestHandler<GetLivestockFeedAnalysisParameterList, Tuple<long, ICollection<LivestockFeedAnalysisParameterModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<LivestockFeedAnalysisParameterModel?>>> Handle(GetLivestockFeedAnalysisParameterList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.LivestockFeedAnalysisParameter>(_context);
                return new Tuple<long, ICollection<LivestockFeedAnalysisParameterModel?>>
                    (await query.LongCountAsync(cancellationToken),
                    await query.Select(f => LivestockFeedAnalysisParameterModel.Create(f)).ToListAsync(cancellationToken));
            }
        }
    }
}
