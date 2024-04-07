using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Treatment
{
    public class CreateTreatment : BaseCommand, ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] required public TreatmentModel Treatment { get; set; }
        public class Handler : BaseCommandHandler<CreateTreatment>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(CreateTreatment request, CancellationToken cancellationToken)
            {
                var treatment = request.Treatment.Map(new Domain.Entity.Treatment(request.ModifiedBy, request.TenantId)) as Domain.Entity.Treatment;
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    context.Treatments.Add(treatment);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new ModifiedEntityPushNotification(request.TenantId, TreatmentModel.Create(treatment).GetJsonString(), nameof(TreatmentModel),treatment.ModifiedOn), cancellationToken);
                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Create Treatment"); }
                }
                return treatment.Id;
            }
    


        }
    }
}
