using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace BackEnd.BusinessLogic.ScheduledDuty
{
    public class UpdateScheduledDuty : BaseCommand, IUpdateCommand
    {
        public ScheduledDutyModel Duty { get; set; }
        public class Handler: BaseCommandHandler<UpdateScheduledDuty>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
            {
            }
            public override async Task<long> Handle(UpdateScheduledDuty request, CancellationToken cancellationToken)
            {
                var duty = _context.ScheduledDuties.First(d => d.TenantId == request.TenantId && d.Id == request.Duty.Id);
                duty = request.Duty.MapToEntity(duty);
                duty.ModifiedOn = DateTime.Now;
                duty.ModifiedBy = request.ModifiedBy;
                await _context.SaveChangesAsync(cancellationToken);
                return duty.Id;
            }
        }
    }
}
