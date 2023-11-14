using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.BreedingRecord
{
    public class GetBreedingRecord : BreedingRecordQueries, IRequest<BreedingRecordModel?>
    {
        public class Handler : BaseRequestHandler<GetBreedingRecord>, IRequestHandler<GetBreedingRecord, BreedingRecordModel?>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }
            public async Task<BreedingRecordModel?> Handle(GetBreedingRecord request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    return BreedingRecordModel.Create(await request.GetQuery<Domain.Entity.BreedingRecord>(context).FirstOrDefaultAsync(cancellationToken));
                }
            }
        }
    }
}
