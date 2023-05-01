﻿using BackEnd.Abstracts;
using BackEnd.Models;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;

namespace BackEnd.BusinessLogic.Milestone
{
    public class UpdateMilestone : BaseCommand, IUpdateCommand
    {
        public MilestoneModel Milestone { get; set; }
        public class Handler: BaseCommandHandler<UpdateMilestone>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
            {
            }
            public override async Task<long> Handle(UpdateMilestone request, CancellationToken cancellationToken)
            {
                var duty = _context.Milestones.First(d => d.TenantId == request.TenantId && d.Id == request.Milestone.Id);
                duty = request.Milestone.MapToEntity(duty);
                duty.ModifiedOn = DateTime.Now;
                duty.ModifiedBy = request.ModifiedBy;
                await _context.SaveChangesAsync(cancellationToken);
                await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(duty.Id.ToString(), duty.GetType().Name, "Modified", duty.ModifiedBy) }), cancellationToken);
                return duty.Id;
            }
        }
    }
}
