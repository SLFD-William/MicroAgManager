using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Treatment
{
    public class UpdateTreatment : BaseCommand, IUpdateCommand
    {
        public required TreatmentModel Treatment { get; set; }
public class Handler: BaseCommandHandler<UpdateTreatment>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateTreatment request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var treatment = request.Treatment.Map(await context.Treatments.FindAsync(request.Treatment.Id)) as Domain.Entity.Treatment;
                    treatment.ModifiedBy = request.ModifiedBy;

                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new ModifiedEntityPushNotification(request.TenantId, TreatmentModel.Create(treatment).GetJsonString(), nameof(TreatmentModel),treatment.ModifiedOn), cancellationToken);

                     }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Update Treatment"); }

                    return treatment.Id;
                }
            }
        }
    }
}
