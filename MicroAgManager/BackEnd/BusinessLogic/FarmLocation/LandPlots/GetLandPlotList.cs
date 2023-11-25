using BackEnd.Abstracts;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.FarmLocation.LandPlots
{
    public class GetLandPlotList : LandPlotQueries, IRequest<LandPlotDto>
    {
        public class Handler : BaseRequestHandler<GetLandPlotList>, IRequestHandler<GetLandPlotList, LandPlotDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<LandPlotDto> Handle(GetLandPlotList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.LandPlot>(context);

                    return new LandPlotDto(await query.LongCountAsync(cancellationToken), await query.Select(f => LandPlotModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
