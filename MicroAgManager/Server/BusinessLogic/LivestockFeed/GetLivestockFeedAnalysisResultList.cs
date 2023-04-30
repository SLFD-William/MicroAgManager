using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.LivestockFeed
{
    public class GetLivestockFeedAnalysisResultList : LivestockFeedAnalysisResultQueries, IRequest<Tuple<long, ICollection<LivestockFeedAnalysisResultModel?>>>
    {
        public class Handler : IRequestHandler<GetLivestockFeedAnalysisResultList, Tuple<long, ICollection<LivestockFeedAnalysisResultModel?>>>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
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
