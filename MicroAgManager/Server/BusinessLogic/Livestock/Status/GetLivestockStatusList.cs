using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.Livestock.Status
{
    public class GetLivestockStatusList : LivestockStatusQueries, IRequest<Tuple<long, ICollection<LivestockStatusModel?>>>
    {
        public class Handler : IRequestHandler<GetLivestockStatusList, Tuple<long, ICollection<LivestockStatusModel?>>>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<Tuple<long, ICollection<LivestockStatusModel?>>> Handle(GetLivestockStatusList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.LivestockStatus>(_context);

                return new Tuple<long, ICollection<LivestockStatusModel?>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => LivestockStatusModel.Create(f)).ToListAsync(cancellationToken)
                    );
            }
        }
    }
}
