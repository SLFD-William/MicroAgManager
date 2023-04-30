using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.LandPlots
{
    public class GetLandPlotList : LandPlotQueries, IRequest<Tuple<long, ICollection<LandPlotModel?>>>
    {
        public class Handler : IRequestHandler<GetLandPlotList, Tuple<long, ICollection<LandPlotModel?>>>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<Tuple<long, ICollection<LandPlotModel?>>> Handle(GetLandPlotList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.LandPlot>(_context);

                return new Tuple<long, ICollection<LandPlotModel?>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => LandPlotModel.Create(f)).ToListAsync(cancellationToken)
                    );
            }
        }
    }
}
