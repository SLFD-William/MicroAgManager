using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Milestone
{
    public class GetMilestoneList:MilestoneQueries, IRequest<Tuple<long, ICollection<MilestoneModel?>>>
    {
        public class Handler : BaseRequestHandler<GetMilestoneList>, IRequestHandler<GetMilestoneList, Tuple<long, ICollection<MilestoneModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
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
