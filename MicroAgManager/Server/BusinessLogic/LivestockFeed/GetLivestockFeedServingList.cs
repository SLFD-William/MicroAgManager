﻿using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.LivestockFeed
{
    public class GetLivestockFeedServingList : LivestockFeedServingQueries, IRequest<Tuple<long, ICollection<LivestockFeedServingModel?>>>
    {
        public class Handler : BaseRequestHandler<GetLivestockFeedServingList>, IRequestHandler<GetLivestockFeedServingList, Tuple<long, ICollection<LivestockFeedServingModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
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
