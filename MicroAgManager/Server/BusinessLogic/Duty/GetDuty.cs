using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.Duty
{
    public class GetDuty : DutyQueries, IRequest<DutyModel?>
    {
        public long Id { get; set; }
        public class Handler : IRequestHandler<GetDuty, DutyModel?>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<DutyModel?> Handle(GetDuty request, CancellationToken cancellationToken) =>
                DutyModel.Create(await request.GetQuery<Domain.Entity.Duty>(_context).FirstOrDefaultAsync(cancellationToken));
        }
    }
}
