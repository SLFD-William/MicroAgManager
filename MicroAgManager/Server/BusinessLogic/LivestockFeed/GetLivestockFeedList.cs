using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.LivestockFeed
{
    public class GetLivestockFeedList : LivestockFeedQueries, IRequest<Tuple<long, ICollection<LivestockFeedModel?>>>
    {
        public class Handler : IRequestHandler<GetLivestockFeedList, Tuple<long, ICollection<LivestockFeedModel?>>>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<Tuple<long, ICollection<LivestockFeedModel?>>> Handle(GetLivestockFeedList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery(_context);
                return new Tuple<long, ICollection<LivestockFeedModel?>>
                    (await query.LongCountAsync(cancellationToken),
                                        await query.Select(f => LivestockFeedModel.Create(f)).ToListAsync(cancellationToken)
                                                           );
            }
        }
    }
}
