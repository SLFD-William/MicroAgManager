using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
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
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(CreateRegistrar request, CancellationToken cancellationToken)
            {
                var registrar = new Domain.Entity.Registrar(request.ModifiedBy, request.TenantId);
                registrar = request.Registrar.MapToEntity(registrar);
                _context.Registrars.Add(registrar);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(registrar.Id.ToString(), registrar.GetType().Name, "Created", registrar.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return registrar.Id;
            }
        }
    }
}
