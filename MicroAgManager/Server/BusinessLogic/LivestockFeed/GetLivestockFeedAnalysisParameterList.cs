using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.LivestockFeed
{
    public class GetLivestockFeedAnalysisParameterList : LivestockFeedAnalysisParameterQueries, IRequest<Tuple<long, ICollection<LivestockFeedAnalysisParameterModel?>>>
    {
        public class Handler : IRequestHandler<GetLivestockFeedAnalysisParameterList, Tuple<long, ICollection<LivestockFeedAnalysisParameterModel?>>>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<Tuple<long, ICollection<LivestockFeedAnalysisParameterModel?>>> Handle(GetLivestockFeedAnalysisParameterList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery(_context);
                return new Tuple<long, ICollection<LivestockFeedAnalysisParameterModel?>>
                    (await query.LongCountAsync(cancellationToken),
                    await query.Select(f => LivestockFeedAnalysisParameterModel.Create(f)).ToListAsync(cancellationToken));
            }
        }
    }
}
