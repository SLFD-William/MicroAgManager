using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Milestone
{
    public class GetMilestone : MilestoneQueries, IRequest<MilestoneModel?>
    {
        public class Handler : BaseRequestHandler<GetMilestone>, IRequestHandler<GetMilestone, MilestoneModel?>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<MilestoneModel?> Handle(GetMilestone request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    return MilestoneModel.Create(await request.GetQuery<Domain.Entity.Milestone>(context).FirstOrDefaultAsync(cancellationToken), context);
                }
            }
        }
    }
}
