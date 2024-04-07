using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Event
{
    public class UpdateEvent : BaseCommand, IUpdateCommand
    {
        public EventModel Event { get; set; }
        public class Handler: BaseCommandHandler<UpdateEvent>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateEvent request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var eventEntity =await context.Events.FirstAsync(d => d.TenantId == request.TenantId && d.Id == request.Event.Id);
                    eventEntity = request.Event.Map(eventEntity) as Domain.Entity.Event;
                    await context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new ModifiedEntityPushNotification (eventEntity.TenantId, EventModel.Create(eventEntity).GetJsonString(), nameof(EventModel), eventEntity.ModifiedOn), cancellationToken); return eventEntity.Id;
                }
            }
        }
    }
}
