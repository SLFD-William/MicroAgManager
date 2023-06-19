using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.ScheduledDuty
{
    public class GetScheduledDuty : ScheduledDutyQueries, IRequest<ScheduledDutyModel?>
    {
        public long Id { get; set; }
        public class Handler : BaseRequestHandler<GetScheduledDuty>, IRequestHandler<GetScheduledDuty, ScheduledDutyModel?>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<ScheduledDutyModel?> Handle(GetScheduledDuty request, CancellationToken cancellationToken) =>
                ScheduledDutyModel.Create(await request.GetQuery<Domain.Entity.ScheduledDuty>(_context).FirstOrDefaultAsync(cancellationToken));
        }
    }
}
