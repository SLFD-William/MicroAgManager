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
    public class GetBreedingRecord : BreedingRecordQueries, IRequest<BreedingRecordModel?>
    {
        public class Handler : BaseRequestHandler<GetBreedingRecord>, IRequestHandler<GetBreedingRecord, BreedingRecordModel?>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }
            public async Task<BreedingRecordModel?> Handle(GetBreedingRecord request, CancellationToken cancellationToken) =>
                BreedingRecordModel.Create(await request.GetQuery<Domain.Entity.BreedingRecord>(_context).FirstOrDefaultAsync(cancellationToken));
        }
    }
}
