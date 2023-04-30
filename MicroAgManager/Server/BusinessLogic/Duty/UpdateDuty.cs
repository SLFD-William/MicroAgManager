using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace BackEnd.BusinessLogic.Duty
{
    public class UpdateDuty : BaseCommand, IUpdateCommand
    {
        public DutyModel Duty { get; set; }
        public class Handler: BaseCommandHandler<UpdateDuty>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
            {
            }
            public override async Task<long> Handle(UpdateDuty request, CancellationToken cancellationToken)
            {
                var duty = _context.Duties.First(d => d.TenantId == request.TenantId && d.Id == request.Duty.Id);
                duty = request.Duty.MapToEntity(duty);
                duty.ModifiedOn = DateTime.Now;
                duty.ModifiedBy = request.ModifiedBy;
                await _context.SaveChangesAsync(cancellationToken);
                return duty.Id;
            }
        }
    }
}
