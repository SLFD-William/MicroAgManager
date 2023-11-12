using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.FarmLocation
{
    public class GetFarmList : FarmLocationsQueries, IRequest<FarmDto>
    {
        public class Handler : BaseRequestHandler<GetFarmList>, IRequestHandler<GetFarmList, FarmDto>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<FarmDto> Handle(GetFarmList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.FarmLocation>(_context);
                return new FarmDto 
                (
                    await query.LongCountAsync(cancellationToken),
                    await query.Select(f => FarmLocationModel.Create(f)).ToListAsync(cancellationToken)
                );
            }
        }
    }
}

