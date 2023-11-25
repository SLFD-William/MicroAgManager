using BackEnd.Abstracts;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Measurement
{
    public class GetMeasurementList : MeasurementQueries, IRequest<MeasurementDto>
    {
        public class Handler : BaseRequestHandler<GetMeasurementList>, IRequestHandler<GetMeasurementList, MeasurementDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<MeasurementDto> Handle(GetMeasurementList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.Measurement>(context);
                    return new MeasurementDto(await query.LongCountAsync(cancellationToken), await query.Select(f => MeasurementModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }

    }
}
