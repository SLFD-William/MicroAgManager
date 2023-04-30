using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.ScheduledDuty
{
    public class CreateScheduledDuty : BaseCommand, ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public ScheduledDutyModel ScheduledDuty { get; set; }

        public class Handler:BaseCommandHandler<CreateScheduledDuty>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
            {
            }
            public override async Task<long> Handle(CreateScheduledDuty request, CancellationToken cancellationToken)
            {
                var duty = new Domain.Entity.ScheduledDuty(request.ModifiedBy, request.TenantId);
                duty = request.ScheduledDuty.MapToEntity(duty);
                duty.ModifiedOn = duty.Created = DateTime.Now;
                duty.ModifiedBy = duty.CreatedBy = request.ModifiedBy;
                duty.TenantId = request.TenantId;
                _context.ScheduledDuties.Add(duty);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return duty.Id;
            }
        }

    }
}
