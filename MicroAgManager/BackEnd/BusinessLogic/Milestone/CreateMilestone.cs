using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Milestone
{
    public class CreateMilestone : BaseCommand, ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] required public MilestoneModel Milestone { get; set; }

        public class Handler:BaseCommandHandler<CreateMilestone>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(CreateMilestone request, CancellationToken cancellationToken)
            {
                var milestone = request.Milestone.Map(new Domain.Entity.Milestone(request.ModifiedBy, request.TenantId)) as Domain.Entity.Milestone;
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    context.Milestones.Add(milestone);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new ModifiedEntityPushNotification(request.TenantId, MilestoneModel.Create(milestone).GetJsonString(), nameof(MilestoneModel), milestone.ModifiedOn), cancellationToken);
                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Create Milestone"); }
                }
                return milestone.Id;
            }
        }

    }
}
