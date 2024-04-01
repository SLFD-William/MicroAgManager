using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Milestone
{
    public class UpdateMilestone : BaseCommand, IUpdateCommand
    {
        public required MilestoneModel Milestone { get; set; }
        public class Handler: BaseCommandHandler<UpdateMilestone>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateMilestone request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var milestone = await context.Milestones.Include(d => d.Duties).FirstAsync(d => d.TenantId == request.TenantId && d.Id == request.Milestone.Id);
                    milestone = request.Milestone.Map(milestone) as Domain.Entity.Milestone;
                    milestone.ModifiedBy = request.ModifiedBy;
                    var dutyIds = request.Milestone.Duties.Select(d => d.Id).ToList();
                    var dutiesToDelete = milestone.Duties.Where(d => !dutyIds.Contains(d.Id)).ToList();
                    var dutiesToAdd = dutyIds.Where(id => !milestone.Duties.Select(x => x.Id).ToList().Contains(id));

                    foreach (var dut in dutiesToDelete)
                        milestone.Duties.Remove(dut);
                    
                    var duties = await context.Duties.Where(d => dutiesToAdd.Contains(d.Id)).ToListAsync();

                    foreach (var duty in duties)
                        milestone.Duties.Add(duty);

                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new ModifiedEntityPushNotification(request.TenantId, MilestoneModel.Create(milestone).GetJsonString(), nameof(MilestoneModel)), cancellationToken);

                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Update Milestone"); }
                    return milestone.Id;
                }
            }
        }
    }
}
