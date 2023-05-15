using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Event
{
    public class CreateEvent : BaseCommand, ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public EventModel Event { get; set; }

        public class Handler:BaseCommandHandler<CreateEvent>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
            {
            }
            public override async Task<long> Handle(CreateEvent request, CancellationToken cancellationToken)
            {
                var eventEntity = new Domain.Entity.Event(request.ModifiedBy, request.TenantId);
                eventEntity = request.Event.MapToEntity(eventEntity);
                eventEntity.ModifiedOn = eventEntity.CreatedOn = DateTime.Now;
                eventEntity.ModifiedBy = eventEntity.CreatedBy = request.ModifiedBy;
                eventEntity.TenantId = request.TenantId;
                _context.Events.Add(eventEntity);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, 
                        new() { new ModifiedEntity(eventEntity.Id.ToString(),eventEntity.GetType().Name, "Created", eventEntity.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return eventEntity.Id;
            }
        }

    }
}
