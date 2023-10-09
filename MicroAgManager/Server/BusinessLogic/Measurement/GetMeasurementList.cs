using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Measure
{
    public class GetMeasurementList : MeasurementQueries, IRequest<Tuple<long, ICollection<MeasurementModel?>>>
    {
        public class Handler : BaseRequestHandler<GetMeasurementList>, IRequestHandler<GetMeasurementList, Tuple<long, ICollection<MeasurementModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<MeasurementModel?>>> Handle(GetMeasurementList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.Measurement>(_context);
                return new Tuple<long, ICollection<MeasurementModel?>>
                    (await query.LongCountAsync(cancellationToken),
                                       await query.Select(f => MeasurementModel.Create(f)).ToListAsync(cancellationToken));
            }
        }

    }
}
