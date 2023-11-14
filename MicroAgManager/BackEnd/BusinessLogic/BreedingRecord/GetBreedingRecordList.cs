using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.BreedingRecord
{
    public class GetBreedingRecordList:BreedingRecordQueries, IRequest<BreedingRecordDto>
    {
        public class Handler : BaseRequestHandler<GetBreedingRecordList>, IRequestHandler<GetBreedingRecordList, BreedingRecordDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<BreedingRecordDto> Handle(GetBreedingRecordList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.BreedingRecord>(context);
                    return new BreedingRecordDto(await query.LongCountAsync(cancellationToken), await query.Select(f => BreedingRecordModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
