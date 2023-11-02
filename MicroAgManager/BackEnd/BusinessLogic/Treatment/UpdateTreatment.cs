using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Treatment
{
    public class UpdateTreatment : BaseCommand, IUpdateCommand
    {
        public required TreatmentModel Treatment { get; set; }
public class Handler: BaseCommandHandler<UpdateTreatment>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateTreatment request, CancellationToken cancellationToken)
            {
                var treatment = request.Treatment.Map(await _context.Treatments.FindAsync(request.Treatment.Id)) as Domain.Entity.Treatment;
                treatment.ModifiedBy = request.ModifiedBy;
               
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(treatment.Id.ToString(), treatment.GetType().Name, "Modified", treatment.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Update Treatment"); }

                return treatment.Id;
            }
        }
    }
}
