﻿using BackEnd.Abstracts;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Feed
{
    public class GetLivestockFeedAnalysisResultList : LivestockFeedAnalysisResultQueries, IRequest<Tuple<long, ICollection<LivestockFeedAnalysisResultModel?>>>
    {
        public class Handler : BaseRequestHandler<GetLivestockFeedAnalysisResultList>, IRequestHandler<GetLivestockFeedAnalysisResultList, Tuple<long, ICollection<LivestockFeedAnalysisResultModel?>>>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<LivestockFeedAnalysisResultModel?>>> Handle(GetLivestockFeedAnalysisResultList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.LivestockFeedAnalysisResult>(context);
                    return new Tuple<long, ICollection<LivestockFeedAnalysisResultModel?>>(await query.LongCountAsync(cancellationToken), await query.Select(f => LivestockFeedAnalysisResultModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
