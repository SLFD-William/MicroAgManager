using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Status
{
    public class GetLivestockStatusList : LivestockStatusQueries, IRequest<LivestockStatusDto>
    {
        public class Handler : BaseRequestHandler<GetLivestockStatusList>, IRequestHandler<GetLivestockStatusList, LivestockStatusDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<LivestockStatusDto> Handle(GetLivestockStatusList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.LivestockStatus>(context);

                    return new LivestockStatusDto(await query.LongCountAsync(cancellationToken), await query.Select(f => LivestockStatusModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
