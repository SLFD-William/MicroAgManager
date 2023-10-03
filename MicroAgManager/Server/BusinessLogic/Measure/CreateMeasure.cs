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
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(CreateMeasure request, CancellationToken cancellationToken)
            {
                var measure = new Domain.Entity.Measure(request.ModifiedBy, request.TenantId);
                measure = request.Measure.MapToEntity(measure);
                _context.Measures.Add(measure);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(measure.Id.ToString(), measure.GetType().Name, "Created", measure.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Create Measure"); }
                return measure.Id;
            }
        }


    }
}
