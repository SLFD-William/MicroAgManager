using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Logic;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.ScheduledDuty
{
    public class UpdateScheduledDuty : BaseCommand, IUpdateCommand
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
                    var duty = request.ScheduledDuty.Map(await context.ScheduledDuties.FirstAsync(d => d.TenantId == request.TenantId && d.Id == request.ScheduledDuty.Id)) as Domain.Entity.ScheduledDuty;
                    duty.ModifiedBy = request.ModifiedBy;
                    await context.SaveChangesAsync(cancellationToken);
                    if (request.Reschedule == true && request.RescheduleDueOn.HasValue && duty.CompletedOn.HasValue)
                    {
                        var newDuty = await DutyLogic.RescheduleDuty(context, duty.Id, request.RescheduleDueOn.Value);
                        var command = new CreateScheduledDuty() { 
                            CreatedBy = newDuty.CreatedBy, 
                            ScheduledDuty = newDuty.ScheduledDuty, 
                            TenantId = request.TenantId,
                            ModifiedBy=request.ModifiedBy};
                        await _mediator.Send(command, cancellationToken);
                    }
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(duty.Id.ToString(), duty.GetType().Name, "Modified", duty.ModifiedBy, duty.ModifiedOn) }), cancellationToken);
                    return duty.Id;
                }
            }
        }
    }
}
