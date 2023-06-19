using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock
{
    public class GetLivestockList : LivestockQueries, IRequest<Tuple<long, ICollection<LivestockModel?>>>
    {
        public class Handler : BaseRequestHandler<GetLivestockList>, IRequestHandler<GetLivestockList, Tuple<long, ICollection<LivestockModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<LivestockModel?>>> Handle(GetLivestockList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.Livestock>(_context);

                return new Tuple<long, ICollection<LivestockModel?>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => LivestockModel.Create(f)).ToListAsync(cancellationToken)
                    );
            }
        }
    }
}
