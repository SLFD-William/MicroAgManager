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
    public class CreateMeasurement : BaseCommand, ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] required public MeasurementModel Measurement { get; set; }

        public class Handler:BaseCommandHandler<CreateMeasurement>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(CreateMeasurement request, CancellationToken cancellationToken)
            {
                var measurement = new Domain.Entity.Measurement(request.ModifiedBy, request.TenantId);
                measurement = request.Measurement.MapToEntity(measurement);
                _context.Measurements.Add(measurement);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(measurement.Id.ToString(), measurement.GetType().Name, "Created", measurement.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Create Measurement"); }
                return measurement.Id;
            }
        }


    }
}
