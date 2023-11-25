using BackEnd.Abstracts;
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
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<LivestockFeedModel?>>> Handle(GetLivestockFeedList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.LivestockFeed>(context);
                    return new Tuple<long, ICollection<LivestockFeedModel?>>(await query.LongCountAsync(cancellationToken), await query.Select(f => LivestockFeedModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
