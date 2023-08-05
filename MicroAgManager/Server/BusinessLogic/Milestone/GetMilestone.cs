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
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<MilestoneModel?> Handle(GetMilestone request, CancellationToken cancellationToken) =>
                MilestoneModel.Create(await request.GetQuery<Domain.Entity.Milestone>(_context).FirstOrDefaultAsync(cancellationToken));
        }
    }
}
