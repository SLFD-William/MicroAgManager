using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Feed
{
    public class GetLivestockFeedServingList : LivestockFeedServingQueries, IRequest<Tuple<long, ICollection<LivestockFeedServingModel?>>>
    {
        public class Handler : BaseRequestHandler<GetLivestockFeedServingList>, IRequestHandler<GetLivestockFeedServingList, Tuple<long, ICollection<LivestockFeedServingModel?>>>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<LivestockFeedServingModel?>>> Handle(GetLivestockFeedServingList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.LivestockFeedServing>(context);
                    return new Tuple<long, ICollection<LivestockFeedServingModel?>>(await query.LongCountAsync(cancellationToken), await query.Select(f => LivestockFeedServingModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
