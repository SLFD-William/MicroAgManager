using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Measure
{
    public class UpdateMeasure : BaseCommand, IUpdateCommand
    {
        public required MeasureModel Measure { get; set; }
        public class Handler: BaseCommandHandler<UpdateMeasure>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateMeasure request, CancellationToken cancellationToken)
            {
                var measure = request.Measure.Map(await _context.Measures.FindAsync(request.Measure.Id)) as Domain.Entity.Measure;
                measure.ModifiedBy = request.ModifiedBy;
               
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(measure.Id.ToString(), measure.GetType().Name, "Modified", measure.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Update Measure"); }

                return measure.Id;
            }
        }
    }
}
