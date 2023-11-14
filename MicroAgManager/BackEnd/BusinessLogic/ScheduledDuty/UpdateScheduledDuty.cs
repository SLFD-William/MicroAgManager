using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.ScheduledDuty
{
    public class UpdateScheduledDuty : BaseCommand, IUpdateCommand
    {
        public ScheduledDutyModel Duty { get; set; }
        public class Handler: BaseCommandHandler<UpdateScheduledDuty>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateScheduledDuty request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var duty = request.Duty.Map(await context.ScheduledDuties.FirstAsync(d => d.TenantId == request.TenantId && d.Id == request.Duty.Id)) as Domain.Entity.ScheduledDuty;
                    duty.ModifiedBy = request.ModifiedBy;
                    await context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(duty.Id.ToString(), duty.GetType().Name, "Modified", duty.ModifiedBy) }), cancellationToken);
                    return duty.Id;
                }
            }
        }
    }
}
