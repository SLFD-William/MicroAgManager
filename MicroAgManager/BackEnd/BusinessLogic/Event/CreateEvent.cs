using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Event
{
    public class CreateEvent : BaseCommand, ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public EventModel Event { get; set; }

        public class Handler:BaseCommandHandler<CreateEvent>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(CreateEvent request, CancellationToken cancellationToken)
            {
                var eventEntity = new Domain.Entity.Event(request.ModifiedBy, request.TenantId);
                eventEntity = request.Event.Map(eventEntity) as Domain.Entity.Event;
                eventEntity.ModifiedBy = eventEntity.CreatedBy = request.ModifiedBy;
                eventEntity.TenantId = request.TenantId;
                _context.Events.Add(eventEntity);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, 
                        new() { new ModifiedEntity(eventEntity.Id.ToString(),eventEntity.GetType().Name, "Created", eventEntity.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Create Event"); }
                return eventEntity.Id;
            }
        }

    }
}
