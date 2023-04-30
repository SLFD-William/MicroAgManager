using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.Livestock
{
    public class GetLivestockList : LivestockQueries, IRequest<Tuple<long, ICollection<LivestockModel?>>>
    {
        public class Handler : IRequestHandler<GetLivestockList, Tuple<long, ICollection<LivestockModel?>>>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
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
