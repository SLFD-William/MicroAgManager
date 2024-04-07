using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Registrar
{
    public class UpdateRegistrar:BaseCommand,IUpdateCommand
    {
        public RegistrarModel Registrar { get; set; }
        public class Handler : BaseCommandHandler<UpdateRegistrar>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateRegistrar request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var registrar = await context.Registrars.FirstAsync(r=>r.Id==request.Registrar.Id && request.TenantId==r.TenantId);
                    if (registrar is null) throw new KeyNotFoundException($"Registrar {request.Registrar.Id} not found.");
                    registrar = request.Registrar.Map(registrar) as Domain.Entity.Registrar;
                    context.Registrars.Update(registrar);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new ModifiedEntityPushNotification(request.TenantId, RegistrarModel.Create(registrar).GetJsonString(), nameof(RegistrarModel), registrar.ModifiedOn), cancellationToken);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                    return registrar.Id;
                }
            }
        }
    }
}
