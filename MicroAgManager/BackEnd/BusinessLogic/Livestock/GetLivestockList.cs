using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock
{
    public class GetLivestockList : LivestockQueries, IRequest<LivestockDto>
    {
        public class Handler : BaseRequestHandler<GetLivestockList>, IRequestHandler<GetLivestockList, LivestockDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<LivestockDto> Handle(GetLivestockList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.Livestock>(context);
                    var data= new LivestockDto(await query.LongCountAsync(cancellationToken), await query.Select(f => LivestockModel.Create(f)).ToListAsync(cancellationToken));
                    return data;
                }
            }
        }
    }
}
