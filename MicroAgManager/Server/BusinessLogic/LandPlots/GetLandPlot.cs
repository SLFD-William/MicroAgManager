using BackEnd.Abstracts;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.LandPlots
{
    public class GetLandPlot : LandPlotQueries, IRequest<LandPlotModel?>
    {

        public class Handler : BaseRequestHandler<GetLandPlot>, IRequestHandler<GetLandPlot, LandPlotModel?>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<LandPlotModel?> Handle(GetLandPlot request, CancellationToken cancellationToken) =>
                LandPlotModel.Create(await request.GetQuery<LandPlot>(_context).FirstOrDefaultAsync(cancellationToken));
        }
    }
}

