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
        public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
        {
        }

        public override async Task<long> Handle(CreateDuty request, CancellationToken cancellationToken)
        {
            var duty = new Domain.Entity.Duty(request.ModifiedBy, request.TenantId);
            duty = request.Duty.MapToEntity(duty);
            duty.ModifiedOn = duty.CreatedOn = DateTime.Now;
            duty.ModifiedBy = duty.CreatedBy = request.ModifiedBy;
            duty.TenantId = request.TenantId;
            _context.Duties.Add(duty);
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, 
                    new() { new ModifiedEntity(duty.Id.ToString(), duty.GetType().Name, "Created", duty.ModifiedBy) }), cancellationToken);
            }
            catch (Exception ex) { _log.LogError(ex, "error creating Duty"); }
            return duty.Id;
        }
    }

}
