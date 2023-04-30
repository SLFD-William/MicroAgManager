using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.Milestone
{
    public class GetMilestoneList:MilestoneQueries, IRequest<Tuple<long, ICollection<MilestoneModel?>>>
    {
        public class Handler : IRequestHandler<GetMilestoneList, Tuple<long, ICollection<MilestoneModel?>>>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<Tuple<long, ICollection<MilestoneModel?>>> Handle(GetMilestoneList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.Milestone>(_context);
                return new Tuple<long, ICollection<MilestoneModel?>>
                    (await query.LongCountAsync(cancellationToken),
                    await query.Select(f => MilestoneModel.Create(f)).ToListAsync(cancellationToken));
            }
        }
    }
}
