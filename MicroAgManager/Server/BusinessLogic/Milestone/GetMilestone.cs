using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.Milestone
{
    public class GetMilestone : MilestoneQueries, IRequest<MilestoneModel?>
    {
        public long Id { get; set; }
        public class Handler : IRequestHandler<GetMilestone, MilestoneModel?>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<MilestoneModel?> Handle(GetMilestone request, CancellationToken cancellationToken) =>
                MilestoneModel.Create(await request.GetQuery<Domain.Entity.Milestone>(_context).FirstOrDefaultAsync(cancellationToken));
        }
    }
}
