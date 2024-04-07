using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Duty
{
    public class UpdateDuty : BaseCommand, IUpdateCommand
    {
        public DutyModel Duty { get; set; }
        public class Handler: BaseCommandHandler<UpdateDuty>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateDuty request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var duty =await context.Duties.FirstAsync(d => d.TenantId == request.TenantId && d.Id == request.Duty.Id);
                    duty = request.Duty.Map(duty) as Domain.Entity.Duty;
                    duty.ModifiedBy = request.ModifiedBy;
                    await context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new ModifiedEntityPushNotification(duty.TenantId, DutyModel.Create(duty).GetJsonString(), nameof(DutyModel), duty.ModifiedOn), cancellationToken); 
                    return duty.Id;
                }
            }
        }
    }
}
