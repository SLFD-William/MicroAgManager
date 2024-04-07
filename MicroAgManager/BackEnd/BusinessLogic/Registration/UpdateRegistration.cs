using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
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
                        await _mediator.Publish(new ModifiedEntityPushNotification(request.TenantId, RegistrationModel.Create(registration).GetJsonString(), nameof(RegistrationModel), registration.ModifiedOn), cancellationToken);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                    return registration.Id;
                }
            }
        }
    }
}
