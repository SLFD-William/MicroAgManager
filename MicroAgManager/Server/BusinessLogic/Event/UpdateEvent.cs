using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace BackEnd.BusinessLogic.Event
{
    public class UpdateEvent : BaseCommand, IUpdateCommand
    {
        public EventModel Event { get; set; }
        public class Handler: BaseCommandHandler<UpdateEvent>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
            {
            }
            public override async Task<long> Handle(UpdateEvent request, CancellationToken cancellationToken)
            {
                var Event = _context.Events.First(d => d.TenantId == request.TenantId && d.Id == request.Event.Id);
                Event = request.Event.MapToEntity(Event);
                Event.ModifiedOn = DateTime.Now;
                Event.ModifiedBy = request.ModifiedBy;
                await _context.SaveChangesAsync(cancellationToken);
                return Event.Id;
            }
        }
    }
}
