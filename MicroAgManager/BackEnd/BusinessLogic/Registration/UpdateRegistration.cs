using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Registration
{
    public class UpdateRegistration : BaseCommand, IUpdateCommand
    {
        public RegistrationModel Registration { get; set; }
        public class Handler : BaseCommandHandler<UpdateRegistration>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateRegistration request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var registration = await context.Registrations.FindAsync(request.Registration.Id);
                    if (registration is null) throw new KeyNotFoundException($"Registration {request.Registration.Id} not found.");
                    registration = request.Registration.Map(registration) as Domain.Entity.Registration;
                    context.Registrations.Update(registration);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(registration.Id.ToString(), registration.GetType().Name, "Updated", registration.ModifiedBy) }), cancellationToken);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                    return registration.Id;
                }
            }
        }
    }
}
