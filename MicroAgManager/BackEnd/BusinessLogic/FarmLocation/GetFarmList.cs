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
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<FarmDto> Handle(GetFarmList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.FarmLocation>(context);
                    return new FarmDto(await query.LongCountAsync(cancellationToken), await query.Select(f => FarmLocationModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}

