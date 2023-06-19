using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.LandPlots
{
    public class GetLandPlotList : LandPlotQueries, IRequest<Tuple<long, ICollection<LandPlotModel?>>>
    {
        public class Handler : BaseRequestHandler<GetLandPlotList>, IRequestHandler<GetLandPlotList, Tuple<long, ICollection<LandPlotModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
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
