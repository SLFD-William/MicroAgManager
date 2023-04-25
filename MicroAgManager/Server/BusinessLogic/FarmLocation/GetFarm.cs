using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Server.BusinessLogic.FarmLocation
{
    public class GetFarm : FarmLocationsQueries, IRequest<FarmLocationModel?>
    {

        public class Handler : IRequestHandler<GetFarm, FarmLocationModel?>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<FarmLocationModel?> Handle(GetFarm request, CancellationToken cancellationToken)=>
                FarmLocationModel.Create(await request.GetQuery(_context).FirstOrDefaultAsync(cancellationToken));
        }
    }
}

