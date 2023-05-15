using BackEnd.Abstracts;
using BackEnd.Models;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Milestone
{
    public class CreateMilestone : BaseCommand, ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public MilestoneModel Milestone { get; set; }

        public class Handler:BaseCommandHandler<CreateMilestone>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
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
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return duty.Id;
            }
        }

    }
}
