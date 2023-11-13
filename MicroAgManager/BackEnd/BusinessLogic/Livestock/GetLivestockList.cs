using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock
{
    public class GetLivestockList : LivestockQueries, IRequest<LivestockDto>
    {
        public class Handler : BaseRequestHandler<GetLivestockList>, IRequestHandler<GetLivestockList, LivestockDto>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<LivestockDto> Handle(GetLivestockList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.Livestock>(_context);

                return new LivestockDto
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => LivestockModel.Create(f)).ToListAsync(cancellationToken)
                    );
            }
        }
    }
}
