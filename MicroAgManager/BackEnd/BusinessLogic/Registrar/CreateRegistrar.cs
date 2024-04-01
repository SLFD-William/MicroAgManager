using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Registrar
{
    public class CreateRegistrar:BaseCommand,ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required]public RegistrarModel Registrar { get; set; }
        public class Handler : BaseCommandHandler<CreateRegistrar>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(CreateRegistrar request, CancellationToken cancellationToken)
            {
                var registrar = request.Registrar.Map(new Domain.Entity.Registrar(request.ModifiedBy, request.TenantId)) as Domain.Entity.Registrar;
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    context.Registrars.Add(registrar);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new ModifiedEntityPushNotification(request.TenantId, RegistrarModel.Create(registrar).GetJsonString(), nameof(RegistrarModel)), cancellationToken);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                }
                return registrar.Id;
            }
        }
    }
}
