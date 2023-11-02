using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.TreatmentRecord
{
    public class GetTreatmentRecordList:TreatmentRecordQueries, IRequest<Tuple<long, ICollection<TreatmentRecordModel?>>>
    {
        public class Handler : BaseRequestHandler<GetTreatmentRecordList>, IRequestHandler<GetTreatmentRecordList, Tuple<long, ICollection<TreatmentRecordModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<TreatmentRecordModel?>>> Handle(GetTreatmentRecordList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.TreatmentRecord>(_context);
                return new Tuple<long, ICollection<TreatmentRecordModel?>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => TreatmentRecordModel.Create(f)).ToListAsync(cancellationToken));
            }
        }
    }
}
