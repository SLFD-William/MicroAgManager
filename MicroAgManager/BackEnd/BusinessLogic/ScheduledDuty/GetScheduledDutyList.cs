using BackEnd.Abstracts;
using Domain.Interfaces;
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
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<ScheduledDutyDto> Handle(GetScheduledDutyList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.ScheduledDuty>(_context);
                return new ScheduledDutyDto
                    (await query.LongCountAsync(cancellationToken),
                    await query.Select(f => ScheduledDutyModel.Create(f)).ToListAsync(cancellationToken));
            }
        }
    }
}
