using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Status
{
    public class GetLivestockStatusList : LivestockStatusQueries, IRequest<Tuple<long, ICollection<LivestockStatusModel?>>>
    {
        public class Handler : BaseRequestHandler<GetLivestockStatusList>, IRequestHandler<GetLivestockStatusList, Tuple<long, ICollection<LivestockStatusModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<LivestockStatusModel?>>> Handle(GetLivestockStatusList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.LivestockStatus>(_context);

                return new Tuple<long, ICollection<LivestockStatusModel?>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => LivestockStatusModel.Create(f)).ToListAsync(cancellationToken)
                    );
            }
        }
    }
}
