using BackEnd.Abstracts;
using Domain.Interfaces;
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
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<LandPlotDto> Handle(GetLandPlotList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.LandPlot>(_context);

                return new LandPlotDto
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => LandPlotModel.Create(f)).ToListAsync(cancellationToken)
                    );
            }
        }
    }
}
