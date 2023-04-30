using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.Livestock.Types
{
    public class GetLivestockTypeList : LivestockTypeQueries, IRequest<Tuple<long, ICollection<LivestockTypeModel?>>>
    {
        public class Handler : IRequestHandler<GetLivestockTypeList, Tuple<long, ICollection<LivestockTypeModel?>>>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<Tuple<long, ICollection<LivestockTypeModel?>>> Handle(GetLivestockTypeList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.LivestockType>(_context);

                return new Tuple<long, ICollection<LivestockTypeModel?>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => LivestockTypeModel.Create(f)).ToListAsync(cancellationToken)
                    );
            }
        }
    }
}
