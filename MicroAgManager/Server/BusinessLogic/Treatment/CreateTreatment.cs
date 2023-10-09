using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
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
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(CreateTreatment request, CancellationToken cancellationToken)
            {
                var treatment = request.Treatment.Map(new Domain.Entity.Treatment(request.ModifiedBy, request.TenantId)) as Domain.Entity.Treatment;
                _context.Treatments.Add(treatment);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(treatment.Id.ToString(), treatment.GetType().Name, "Created", treatment.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Create Treatment"); }
                return treatment.Id;
            }
    


        }
    }
}
