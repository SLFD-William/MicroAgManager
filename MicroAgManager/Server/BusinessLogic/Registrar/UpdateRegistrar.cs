using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Registrar
{
    public class UpdateRegistrar:BaseCommand,IUpdateCommand
    {
        public RegistrarModel Registrar { get; set; }
        public class Handler : BaseCommandHandler<UpdateRegistrar>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateRegistrar request, CancellationToken cancellationToken)
            {
                var registrar = await _context.Registrars.FindAsync(request.Registrar.Id);
                if (registrar is null) throw new KeyNotFoundException($"Registrar {request.Registrar.Id} not found.");
                registrar = request.Registrar.Map(registrar) as Domain.Entity.Registrar;
                _context.Registrars.Update(registrar);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(registrar.Id.ToString(), registrar.GetType().Name, "Updated", registrar.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return registrar.Id;
            }
        }
    }
}
