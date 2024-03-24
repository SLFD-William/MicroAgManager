using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Logic;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.ScheduledDuty
{
    public class UpdateScheduledDuty : BaseCommand, IUpdateCommand, IHasReschedule
    {
        public ScheduledDutyModel ScheduledDuty { get; set; }
        public bool? Reschedule { get; set; }
        public DateTime? RescheduleDueOn { get; set; }
        public class Handler: BaseCommandHandler<UpdateScheduledDuty>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateScheduledDuty request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var duty = await context.ScheduledDuties.FirstAsync(d => d.TenantId == request.TenantId && d.Id == request.ScheduledDuty.Id);
                    var originalDuty = duty.Clone() as Domain.Entity.ScheduledDuty;
                    request.ScheduledDuty.Map(duty);
                    duty.ModifiedBy = request.ModifiedBy;
                    await context.SaveChangesAsync(cancellationToken);
                    var notice = new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(duty.Id.ToString(), duty.GetType().Name, "Modified", duty.ModifiedBy, duty.ModifiedOn) });
                    if (originalDuty.CompletedOn != duty.CompletedOn && duty.CompletedOn.HasValue)
                    {
                        var command = await ScheduledDutyLogic.OnCompleted(context, request, duty);
                        if (command is ICreateScheduledDuty)
                        {
                            var rescheduled = command.ScheduledDuty.Map(new Domain.Entity.ScheduledDuty(request.ModifiedBy, request.TenantId)) as Domain.Entity.ScheduledDuty;
                            context.ScheduledDuties.Add(rescheduled);
                            await context.SaveChangesAsync(cancellationToken);
                            notice.EntitiesModified.Add(new ModifiedEntity(rescheduled.Id.ToString(), rescheduled.GetType().Name, "Created", rescheduled.ModifiedBy, rescheduled.ModifiedOn));
                        }
                    }
                    await _mediator.Publish(notice, cancellationToken);
                    return duty.Id;
                }
            }
        }
    }
}
