using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Unit
{
    public class CreateUnit:BaseCommand,ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] required public UnitModel Unit { get ; set ; }

        public class Handler:BaseCommandHandler<CreateUnit>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(CreateUnit request, CancellationToken cancellationToken)
            {
                var unit = request.Unit.Map(new Domain.Entity.Unit(request.ModifiedBy, request.TenantId) { ConversionFactorToSIUnit = 1 }) as Domain.Entity.Unit;
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    context.Units.Add(unit);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(unit.Id.ToString(), unit.GetType().Name, "Created", unit.ModifiedBy, unit.ModifiedOn) }), cancellationToken);
                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Create Unit"); }
                }
                return unit.Id;
            }
        }
    }
}
