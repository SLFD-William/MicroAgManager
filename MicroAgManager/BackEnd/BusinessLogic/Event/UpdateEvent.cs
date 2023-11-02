using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Event
{
    public class UpdateEvent : BaseCommand, IUpdateCommand
    {
        public EventModel Event { get; set; }
        public class Handler: BaseCommandHandler<UpdateEvent>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateEvent request, CancellationToken cancellationToken)
            {
                var eventEntity = _context.Events.First(d => d.TenantId == request.TenantId && d.Id == request.Event.Id);
                eventEntity = request.Event.Map(eventEntity) as Domain.Entity.Event;
                await _context.SaveChangesAsync(cancellationToken);
                await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(eventEntity.Id.ToString(), eventEntity.GetType().Name, "Modified", eventEntity.ModifiedBy) }), cancellationToken);
                return eventEntity.Id;
            }
        }
    }
}
