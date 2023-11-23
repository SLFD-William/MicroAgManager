using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Milestone
{
    public class GetMilestoneList:MilestoneQueries, IRequest<MilestoneDto>
    {
        public class Handler : BaseRequestHandler<GetMilestoneList>, IRequestHandler<GetMilestoneList, MilestoneDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<MilestoneDto> Handle(GetMilestoneList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.Milestone>(context);
                    return new MilestoneDto(await query.LongCountAsync(cancellationToken), await query.Select(f => MilestoneModel.Create(f, context)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
