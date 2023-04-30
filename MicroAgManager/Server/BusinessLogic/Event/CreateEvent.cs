using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
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
                var Event = new Domain.Entity.Event(request.ModifiedBy, request.TenantId);
                Event = request.Event.MapToEntity(Event);
                Event.ModifiedOn = Event.Created = DateTime.Now;
                Event.ModifiedBy = Event.CreatedBy = request.ModifiedBy;
                Event.TenantId = request.TenantId;
                _context.Events.Add(Event);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return Event.Id;
            }
        }

    }
}
