using BackEnd.Abstracts;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Chore
{
    public class GetChore : ChoreQueries, IRequest<ChoreModel?>
    {
        public class Handler : BaseRequestHandler<GetChore>, IRequestHandler<GetChore, ChoreModel?>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<ChoreModel?> Handle(GetChore request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    return ChoreModel.Create(await request.GetQuery<Domain.Entity.Chore>(context).FirstOrDefaultAsync(cancellationToken));
                }
            }
        }
    }
}
