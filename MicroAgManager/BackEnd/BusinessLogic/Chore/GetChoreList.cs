using BackEnd.Abstracts;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Chore
{
    public class GetChoreList:ChoreQueries, IRequest<ChoreDto>
    {
        public class Handler : BaseRequestHandler<GetChoreList>, IRequestHandler<GetChoreList, ChoreDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<ChoreDto> Handle(GetChoreList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.Chore>(context);
                    return new ChoreDto(await query.LongCountAsync(cancellationToken), await query.Select(f => ChoreModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
