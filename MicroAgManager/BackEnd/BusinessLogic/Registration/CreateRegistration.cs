using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Registration
{
    public class CreateRegistration : BaseCommand, ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        public RegistrationModel Registration { get; set; }
        public class Handler : BaseCommandHandler<CreateRegistration>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(CreateRegistration request, CancellationToken cancellationToken)
            {
                var registration = request.Registration.Map(new Domain.Entity.Registration(request.ModifiedBy, request.TenantId)) as Domain.Entity.Registration;
                _context.Registrations.Add(registration);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(registration.Id.ToString(), registration.GetType().Name, "Created", registration.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return registration.Id;
            }
        }
    }
}
