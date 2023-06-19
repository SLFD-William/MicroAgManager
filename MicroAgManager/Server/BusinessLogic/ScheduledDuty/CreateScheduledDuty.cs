﻿using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.ScheduledDuty
{
    public class CreateScheduledDuty : BaseCommand, ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public ScheduledDutyModel ScheduledDuty { get; set; }

        public class Handler:BaseCommandHandler<CreateScheduledDuty>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(CreateScheduledDuty request, CancellationToken cancellationToken)
            {
                var duty = new Domain.Entity.ScheduledDuty(request.ModifiedBy, request.TenantId);
                duty = request.ScheduledDuty.MapToEntity(duty);
                duty.ModifiedOn = duty.CreatedOn = DateTime.Now;
                duty.ModifiedBy = duty.CreatedBy = request.ModifiedBy;
                duty.TenantId = request.TenantId;
                _context.ScheduledDuties.Add(duty);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(duty.Id.ToString(), duty.GetType().Name, "Created", duty.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Create Scheduled Duty"); }
                return duty.Id;
            }
        }

    }
}
