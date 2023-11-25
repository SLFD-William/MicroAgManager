using BackEnd.Abstracts;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.TreatmentRecord
{
    public class GetTreatmentRecordList:TreatmentRecordQueries, IRequest<TreatmentRecordDto>
    {
        public class Handler : BaseRequestHandler<GetTreatmentRecordList>, IRequestHandler<GetTreatmentRecordList, TreatmentRecordDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<TreatmentRecordDto> Handle(GetTreatmentRecordList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.TreatmentRecord>(context);
                    return new TreatmentRecordDto(await query.LongCountAsync(cancellationToken), await query.Select(f => TreatmentRecordModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
