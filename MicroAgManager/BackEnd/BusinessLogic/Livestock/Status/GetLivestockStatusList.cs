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
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<LivestockStatusDto> Handle(GetLivestockStatusList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.LivestockStatus>(_context);

                return new LivestockStatusDto
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => LivestockStatusModel.Create(f)).ToListAsync(cancellationToken)
                    );
            }
        }
    }
}
