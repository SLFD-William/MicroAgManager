using BackEnd.Abstracts;
using BackEnd.BusinessLogic.Duty;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.BusinessLogic.BreedingRecord
{
    public class GetBreedingRecordList:BreedingRecordQueries, IRequest<Tuple<long, ICollection<BreedingRecordModel?>>>
    {
        public class Handler : BaseRequestHandler<GetBreedingRecordList>, IRequestHandler<GetBreedingRecordList, Tuple<long, ICollection<BreedingRecordModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<BreedingRecordModel?>>> Handle(GetBreedingRecordList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.BreedingRecord>(_context);
                return new Tuple<long, ICollection<BreedingRecordModel?>>
                    (await query.LongCountAsync(cancellationToken),
                    await query.Select(f => BreedingRecordModel.Create(f)).ToListAsync(cancellationToken));
            }
        }
    }
}
