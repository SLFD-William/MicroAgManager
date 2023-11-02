using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Feed
{
    public class GetLivestockFeedList : LivestockFeedQueries, IRequest<Tuple<long, ICollection<LivestockFeedModel?>>>
    {
        public class Handler : BaseRequestHandler<GetLivestockFeedList>, IRequestHandler<GetLivestockFeedList, Tuple<long, ICollection<LivestockFeedModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<LivestockFeedModel?>>> Handle(GetLivestockFeedList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.LivestockFeed>(_context);
                return new Tuple<long, ICollection<LivestockFeedModel?>>
                    (await query.LongCountAsync(cancellationToken),
                                        await query.Select(f => LivestockFeedModel.Create(f)).ToListAsync(cancellationToken)
                                                           );
            }
        }
    }
}
