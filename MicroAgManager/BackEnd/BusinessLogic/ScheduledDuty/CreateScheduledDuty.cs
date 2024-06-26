﻿using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Logic;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.ScheduledDuty
{
    public class CreateScheduledDuty : BaseCommand, ICreateCommand, ICreateScheduledDuty, IHasReschedule
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public ScheduledDutyModel ScheduledDuty { get; set; }
        public bool? Reschedule { get; set; }
        public DateTime? RescheduleDueOn { get; set; }

        public class Handler:BaseCommandHandler<CreateScheduledDuty>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(CreateScheduledDuty request, CancellationToken cancellationToken)
            {
                var duty = request.ScheduledDuty.Map(new Domain.Entity.ScheduledDuty(request.ModifiedBy, request.TenantId)) as Domain.Entity.ScheduledDuty;
                if (duty.CompletedOn.HasValue) duty.CompletedBy = request.CreatedBy;
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    context.ScheduledDuties.Add(duty);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new ModifiedEntityPushNotification(request.TenantId, ScheduledDutyModel.Create(duty).GetJsonString(), nameof(ScheduledDutyModel), duty.ModifiedOn), cancellationToken);
                        if (duty.CompletedOn.HasValue)
                        { 
                            var command = await ScheduledDutyLogic.OnCompleted(context, request,duty);
                            if (command is ICreateScheduledDuty)
                            {
                                var rescheduled=command.ScheduledDuty.Map(new Domain.Entity.ScheduledDuty(request.ModifiedBy, request.TenantId)) as Domain.Entity.ScheduledDuty;
                                context.ScheduledDuties.Add(rescheduled);
                                await context.SaveChangesAsync(cancellationToken);
                                await _mediator.Publish(new ModifiedEntityPushNotification(request.TenantId, ScheduledDutyModel.Create(rescheduled).GetJsonString(), nameof(ScheduledDutyModel), rescheduled.ModifiedOn ), cancellationToken);
                            }
                        }
                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Create Scheduled Duty"); }
                }
                return duty.Id;
            }
        }

    }
}
