using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Measure
{
    public class CreateMeasure : BaseCommand, ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }

        [Required] required public MeasureModel Measure { get; set; }

        public class Handler : BaseCommandHandler<CreateMeasure>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(CreateMeasure request, CancellationToken cancellationToken)
            {
                var measure = request.Measure.Map(new Domain.Entity.Measure(request.ModifiedBy, request.TenantId)) as Domain.Entity.Measure;
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    context.Measures.Add(measure);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(measure.Id.ToString(), measure.GetType().Name, "Created", measure.ModifiedBy, measure.ModifiedOn) }), cancellationToken);
                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Create Measure"); }
                }
                return measure.Id;
            }
        }


    }
}
