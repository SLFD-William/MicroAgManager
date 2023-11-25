using BackEnd.Abstracts;
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
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<FarmLocationModel?> Handle(GetFarm request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    return FarmLocationModel.Create(await request.GetQuery<Domain.Entity.FarmLocation>(context).FirstOrDefaultAsync(cancellationToken));
                }
            }
        }
    }
}

