using BackEnd.Abstracts;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.ScheduledDuty
{
    public class GetScheduledDutyList:ScheduledDutyQueries, IRequest<ScheduledDutyDto>
    {
        public class Handler : BaseRequestHandler<GetScheduledDutyList>, IRequestHandler<GetScheduledDutyList, ScheduledDutyDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<ScheduledDutyDto> Handle(GetScheduledDutyList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.ScheduledDuty>(context);
                    var data= new ScheduledDutyDto(await query.LongCountAsync(cancellationToken), await query.Select(f => ScheduledDutyModel.Create(f)).ToListAsync(cancellationToken));
                    return data;
                }
            }
        }
    }
}
