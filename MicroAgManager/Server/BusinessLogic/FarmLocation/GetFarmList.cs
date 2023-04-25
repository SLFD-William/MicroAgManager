using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.FarmLocation
{
    public class GetFarmList : FarmLocationsQueries, IRequest<Tuple<long, ICollection<FarmLocationModel?>>>
    {
        public class Handler : IRequestHandler<GetFarmList, Tuple<long, ICollection<FarmLocationModel?>>>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<Tuple<long, ICollection<FarmLocationModel?>>> Handle(GetFarmList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery(_context);

                return new Tuple<long, ICollection<FarmLocationModel?>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => FarmLocationModel.Create(f)).ToListAsync(cancellationToken)
                    );
            }
        }
    }
}

