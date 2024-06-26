﻿using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
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
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(CreateRegistration request, CancellationToken cancellationToken)
            {
                var registration = request.Registration.Map(new Domain.Entity.Registration(request.ModifiedBy, request.TenantId)) as Domain.Entity.Registration;
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    context.Registrations.Add(registration);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new ModifiedEntityPushNotification(request.TenantId, RegistrationModel.Create(registration).GetJsonString(), nameof(RegistrationModel), registration.ModifiedOn), cancellationToken);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                }
                return registration.Id;
            }
        }
    }
}
