using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.LivestockFeed
{
    public class GetLivestockFeedServingList : LivestockFeedServingQueries, IRequest<Tuple<long, ICollection<LivestockFeedServingModel?>>>
    {
        public class Handler : IRequestHandler<GetLivestockFeedServingList, Tuple<long, ICollection<LivestockFeedServingModel?>>>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<Tuple<long, ICollection<LivestockFeedServingModel?>>> Handle(GetLivestockFeedServingList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.LivestockFeedServing>(_context);
                return new Tuple<long, ICollection<LivestockFeedServingModel?>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => LivestockFeedServingModel.Create(f)).ToListAsync(cancellationToken)
                                                                                                                     );
            }
        }
    }
}
