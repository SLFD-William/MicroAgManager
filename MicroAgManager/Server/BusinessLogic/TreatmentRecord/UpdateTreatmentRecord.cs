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
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async override Task<long> Handle(UpdateTreatmentRecord request, CancellationToken cancellationToken)
            {
                var treatmentRecord = request.TreatmentRecord.Map(await _context.TreatmentRecords.FindAsync(request.TreatmentRecord.Id)) as Domain.Entity.TreatmentRecord;
                _context.TreatmentRecords.Update(treatmentRecord);
                await _context.SaveChangesAsync(cancellationToken);
                return treatmentRecord.Id;
            }
        }
    }
}
