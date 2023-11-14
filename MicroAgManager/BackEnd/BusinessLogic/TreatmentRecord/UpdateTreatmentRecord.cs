using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.TreatmentRecord
{
    public class UpdateTreatmentRecord : BaseCommand, IUpdateCommand
    {
        public required TreatmentRecordModel TreatmentRecord { get; set; }
        public class Handler : BaseCommandHandler<UpdateTreatmentRecord>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async override Task<long> Handle(UpdateTreatmentRecord request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var treatmentRecord = request.TreatmentRecord.Map(await context.TreatmentRecords.FindAsync(request.TreatmentRecord.Id)) as Domain.Entity.TreatmentRecord;
                    context.TreatmentRecords.Update(treatmentRecord);
                    await context.SaveChangesAsync(cancellationToken);
                    return treatmentRecord.Id;
                }
            }
        }
    }
}
