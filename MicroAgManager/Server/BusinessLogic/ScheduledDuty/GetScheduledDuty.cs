using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.ScheduledDuty
{
    public class GetScheduledDuty : ScheduledDutyQueries, IRequest<ScheduledDutyModel?>
    {
        public long Id { get; set; }
        public class Handler : IRequestHandler<GetScheduledDuty, ScheduledDutyModel?>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<ScheduledDutyModel?> Handle(GetScheduledDuty request, CancellationToken cancellationToken) =>
                ScheduledDutyModel.Create(await request.GetQuery<Domain.Entity.ScheduledDuty>(_context).FirstOrDefaultAsync(cancellationToken));
        }
    }
}
