using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.ScheduledDuty
{
    public class GetScheduledDutyList:ScheduledDutyQueries, IRequest<Tuple<long, ICollection<ScheduledDutyModel?>>>
    {
        public class Handler : BaseRequestHandler<GetScheduledDutyList>, IRequestHandler<GetScheduledDutyList, Tuple<long, ICollection<ScheduledDutyModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<ScheduledDutyModel?>>> Handle(GetScheduledDutyList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.ScheduledDuty>(_context);
                return new Tuple<long, ICollection<ScheduledDutyModel?>>
                    (await query.LongCountAsync(cancellationToken),
                    await query.Select(f => ScheduledDutyModel.Create(f)).ToListAsync(cancellationToken));
            }
        }
    }
}
