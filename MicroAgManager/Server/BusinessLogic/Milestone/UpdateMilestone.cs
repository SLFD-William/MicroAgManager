using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
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
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateMilestone request, CancellationToken cancellationToken)
            {
                var milestone = _context.Milestones.Include(d=>d.Duties).First(d => d.TenantId == request.TenantId && d.Id == request.Milestone.Id);
                milestone = request.Milestone.MapToEntity(milestone);
                milestone.ModifiedBy = request.ModifiedBy;
                var dutyIds=request.Milestone.Duties.Select(d=>d.Id).ToList();
                var dutiesToDelete = milestone.Duties.Where(d => !dutyIds.Contains(d.Id)).ToList();
                var dutiesToAdd = dutyIds.Where(id=> !milestone.Duties.Select(x=>x.Id).ToList().Contains(id) );
     
                foreach (var dut in dutiesToDelete)
                    milestone.Duties.Remove(dut);

                foreach (var id in dutiesToAdd)
                    milestone.Duties.Add(await _context.Duties.FindAsync(id));

                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(milestone.Id.ToString(), milestone.GetType().Name, "Modified", milestone.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Update Milestone"); }

                return milestone.Id;
            }
        }
    }
}
