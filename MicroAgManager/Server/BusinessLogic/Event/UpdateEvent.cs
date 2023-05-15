using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
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
                var eventEntity = _context.Events.First(d => d.TenantId == request.TenantId && d.Id == request.Event.Id);
                eventEntity = request.Event.MapToEntity(eventEntity);
                eventEntity.ModifiedOn = DateTime.Now;
                eventEntity.ModifiedBy = request.ModifiedBy;
                await _context.SaveChangesAsync(cancellationToken);
                await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(eventEntity.Id.ToString(), eventEntity.GetType().Name, "Modified", eventEntity.ModifiedBy) }), cancellationToken);
                return eventEntity.Id;
            }
        }
    }
}
