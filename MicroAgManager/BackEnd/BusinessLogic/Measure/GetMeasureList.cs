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
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<MeasureDto> Handle(GetMeasureList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.Measure>(context);
                    return new MeasureDto(await query.LongCountAsync(cancellationToken), await query.Select(f => MeasureModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }

    }
}
