using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Milestone
{
    public class CreateMilestone : BaseCommand, ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public MilestoneModel Milestone { get; set; }

        public class Handler:BaseCommandHandler<CreateMilestone>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(CreateMilestone request, CancellationToken cancellationToken)
            {
                var duty = new Domain.Entity.Milestone(request.ModifiedBy, request.TenantId);
                duty = request.Milestone.MapToEntity(duty);
                duty.ModifiedOn = duty.CreatedOn = DateTime.Now;
                duty.ModifiedBy = duty.CreatedBy = request.ModifiedBy;
                duty.TenantId = request.TenantId;
                _context.Milestones.Add(duty);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(duty.Id.ToString(), duty.GetType().Name, "Created", duty.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Create Milestone"); }
                return duty.Id;
            }
        }

    }
}
