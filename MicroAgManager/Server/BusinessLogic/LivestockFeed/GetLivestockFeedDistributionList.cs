using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.LivestockFeed
{
    public class GetLivestockFeedDistributionList : LivestockFeedDistributionQueries, IRequest<Tuple<long, ICollection<LivestockFeedDistributionModel?>>>
    {
        public class Handler : IRequestHandler<GetLivestockFeedDistributionList, Tuple<long, ICollection<LivestockFeedDistributionModel?>>>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<Tuple<long, ICollection<LivestockFeedDistributionModel?>>> Handle(GetLivestockFeedDistributionList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery(_context);
                return new Tuple<long, ICollection<LivestockFeedDistributionModel?>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => LivestockFeedDistributionModel.Create(f)).ToListAsync(cancellationToken));
            }
        }
    }
}
