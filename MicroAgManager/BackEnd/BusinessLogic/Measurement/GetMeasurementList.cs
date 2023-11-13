using BackEnd.Abstracts;
using Domain.Interfaces;
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
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<MeasurementDto> Handle(GetMeasurementList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.Measurement>(_context);
                return new MeasurementDto
                    (await query.LongCountAsync(cancellationToken),
                                       await query.Select(f => MeasurementModel.Create(f)).ToListAsync(cancellationToken));
            }
        }

    }
}
