using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Measure
{
    public class UpdateMeasure : BaseCommand, IUpdateCommand
    {
        public required MeasureModel Measure { get; set; }
        public class Handler: BaseCommandHandler<UpdateMeasure>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateMeasure request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var measure = request.Measure.Map(await context.Measures.FirstAsync(m => m.Id == request.Measure.Id && request.TenantId == m.TenantId)) as Domain.Entity.Measure;
                    measure.ModifiedBy = request.ModifiedBy;

                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new ModifiedEntityPushNotification(request.TenantId, MeasureModel.Create(measure).GetJsonString(), nameof(MeasureModel), measure.ModifiedOn), cancellationToken);
                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Update Measure"); }

                    return measure.Id;
                }
            }
        }
    }
}
