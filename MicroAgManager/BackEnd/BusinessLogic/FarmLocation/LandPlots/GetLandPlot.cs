using BackEnd.Abstracts;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.FarmLocation.LandPlots
{
    public class GetLandPlot : LandPlotQueries, IRequest<LandPlotModel?>
    {

        public class Handler : BaseRequestHandler<GetLandPlot>, IRequestHandler<GetLandPlot, LandPlotModel?>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<LandPlotModel?> Handle(GetLandPlot request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    return LandPlotModel.Create(await request.GetQuery<LandPlot>(context).FirstOrDefaultAsync(cancellationToken));
                }
            }
        }
    }
}

