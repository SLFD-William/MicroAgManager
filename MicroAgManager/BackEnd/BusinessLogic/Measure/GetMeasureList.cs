using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Measure
{
    public class GetMeasureList:MeasureQueries, IRequest<MeasureDto>
    {
        public class Handler : BaseRequestHandler<GetMeasureList>, IRequestHandler<GetMeasureList, MeasureDto>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<MeasureDto> Handle(GetMeasureList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.Measure>(_context);
                return new MeasureDto
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => MeasureModel.Create(f)).ToListAsync(cancellationToken));
            }
        }

    }
}
