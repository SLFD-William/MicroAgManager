using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Measurement
{
    public class UpdateMeasurement : BaseCommand, IUpdateCommand
    {
        public required MeasurementModel Measurement { get; set; }
        public class Handler : BaseCommandHandler<UpdateMeasurement>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async override Task<long> Handle(UpdateMeasurement request, CancellationToken cancellationToken)
            {
                var measurement = request.Measurement.Map(await _context.Measurements.FindAsync(request.Measurement.Id)) as Domain.Entity.Measurement;
                _context.Measurements.Update(measurement);
                await _context.SaveChangesAsync(cancellationToken);
                return measurement.Id;
            }
        }
    }
}
