using Domain.Entity;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.LandPlots
{
    public class GetLandPlot : LandPlotQueries, IRequest<LandPlotModel?>
    {

        public class Handler : IRequestHandler<GetLandPlot, LandPlotModel?>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<LandPlotModel?> Handle(GetLandPlot request, CancellationToken cancellationToken) =>
                LandPlotModel.Create(await request.GetQuery<LandPlot>(_context).FirstOrDefaultAsync(cancellationToken));
        }
    }
}

