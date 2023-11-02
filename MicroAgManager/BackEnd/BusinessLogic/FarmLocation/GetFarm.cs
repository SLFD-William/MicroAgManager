using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.FarmLocation
{
    public class GetFarm : FarmLocationsQueries, IRequest<FarmLocationModel?>
    {

        public class Handler : BaseRequestHandler<GetFarm>, IRequestHandler<GetFarm, FarmLocationModel?>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<FarmLocationModel?> Handle(GetFarm request, CancellationToken cancellationToken)=>
                FarmLocationModel.Create(await request.GetQuery<Domain.Entity.FarmLocation>(_context).FirstOrDefaultAsync(cancellationToken));
        }
    }
}

