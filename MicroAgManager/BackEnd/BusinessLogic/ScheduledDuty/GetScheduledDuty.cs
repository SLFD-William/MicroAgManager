using BackEnd.Abstracts;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.ScheduledDuty
{
    public class GetScheduledDuty : ScheduledDutyQueries, IRequest<ScheduledDutyModel?>
    {
        public class Handler : BaseRequestHandler<GetScheduledDuty>, IRequestHandler<GetScheduledDuty, ScheduledDutyModel?>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<ScheduledDutyModel?> Handle(GetScheduledDuty request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    return ScheduledDutyModel.Create(await request.GetQuery<Domain.Entity.ScheduledDuty>(context).FirstOrDefaultAsync(cancellationToken));
                }
            }
        }
    }
}
