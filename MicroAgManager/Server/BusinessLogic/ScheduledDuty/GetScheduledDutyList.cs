using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.ScheduledDuty
{
    public class GetScheduledDutyList:ScheduledDutyQueries, IRequest<Tuple<long, ICollection<ScheduledDutyModel?>>>
    {
        public class Handler : IRequestHandler<GetScheduledDutyList, Tuple<long, ICollection<ScheduledDutyModel?>>>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
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
