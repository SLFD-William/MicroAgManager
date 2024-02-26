using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.TreatmentRecord
{
    public class CreateTreatmentRecord : BaseCommand, ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] required public TreatmentRecordModel TreatmentRecord { get; set; }

        public class Handler : BaseCommandHandler<CreateTreatmentRecord>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(CreateTreatmentRecord request, CancellationToken cancellationToken)
            {
                var treatmentRecord = request.TreatmentRecord.Map(new Domain.Entity.TreatmentRecord(request.ModifiedBy, request.TenantId)) as Domain.Entity.TreatmentRecord;
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    context.TreatmentRecords.Add(treatmentRecord);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(treatmentRecord.Id.ToString(), treatmentRecord.GetType().Name, "Created", treatmentRecord.ModifiedBy, treatmentRecord.ModifiedOn) }), cancellationToken);
                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Create TreatmentRecord"); }
                }
                return treatmentRecord.Id;
            }
        }


    }
}
