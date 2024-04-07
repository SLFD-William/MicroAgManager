using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Duty;

public class CreateDuty : BaseCommand, ICreateCommand
{
    public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
    [Required] public DutyModel Duty { get; set; }

    public class Handler:BaseCommandHandler<CreateDuty>
    {
        public Handler(IMediator mediator, ILogger log) : base(mediator, log)
        {
        }

        public override async Task<long> Handle(CreateDuty request, CancellationToken cancellationToken)
        {
            var duty = new Domain.Entity.Duty(request.ModifiedBy, request.TenantId);
            duty = request.Duty.Map(duty) as Domain.Entity.Duty;
            duty.CreatedOn = DateTime.Now;
            duty.ModifiedBy = duty.CreatedBy = request.ModifiedBy;
            duty.TenantId = request.TenantId;
            using (var context = new DbContextFactory().CreateDbContext())
            {
                context.Duties.Add(duty);
                try
                {
                    await context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new ModifiedEntityPushNotification(duty.TenantId, DutyModel.Create(duty).GetJsonString(), nameof(DutyModel), duty.ModifiedOn), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "error creating Duty"); }
            }
            return duty.Id;
        }
    }

}
