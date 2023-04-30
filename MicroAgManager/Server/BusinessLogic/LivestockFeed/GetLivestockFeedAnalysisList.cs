using BackEnd.BusinessLogic.LivestockFeedAnalysis;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.LivestockFeed
{
    public class GetLivestockFeedAnalysisList : LivestockFeedAnalysisQueries, IRequest<Tuple<long, ICollection<LivestockFeedAnalysisModel?>>>
    {
        public class Handler : IRequestHandler<GetLivestockFeedAnalysisList, Tuple<long, ICollection<LivestockFeedAnalysisModel?>>>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
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
