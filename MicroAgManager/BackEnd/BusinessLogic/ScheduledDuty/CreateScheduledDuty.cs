using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Logic;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.ScheduledDuty
{
    public class CreateScheduledDuty : BaseCommand, ICreateCommand, ICreateScheduledDuty
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
                        if (request.Reschedule == true && request.RescheduleDueOn.HasValue && duty.CompletedOn.HasValue)
                        {
                            var newDuty = await DutyLogic.RescheduleDuty(context, duty.Id, request.RescheduleDueOn.Value);
                            var command = new CreateScheduledDuty()
                            {
                                CreatedBy = newDuty.CreatedBy,
                                ScheduledDuty = newDuty.ScheduledDuty,
                                TenantId = request.TenantId,
                                ModifiedBy = request.ModifiedBy
                            };
                            await _mediator.Send(command, cancellationToken);
                        }

                        await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(duty.Id.ToString(), duty.GetType().Name, "Created", duty.ModifiedBy, duty.ModifiedOn) }), cancellationToken);
                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Create Scheduled Duty"); }
                }
                return duty.Id;
            }
        }

    }
}
