using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Measurement
{
    public class UpdateMeasurement : BaseCommand, IUpdateCommand
    {
        public required MeasurementModel Measurement { get; set; }
        public class Handler : BaseCommandHandler<UpdateMeasurement>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async override Task<long> Handle(UpdateMeasurement request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var measurement = request.Measurement.Map(await context.Measurements.FirstAsync(m=>m.Id==request.Measurement.Id && request.TenantId==m.TenantId)) as Domain.Entity.Measurement;
                    context.Measurements.Update(measurement);
                    await context.SaveChangesAsync(cancellationToken);
                    return measurement.Id;
                }
            }
        }
    }
}
