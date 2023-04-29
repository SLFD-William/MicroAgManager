using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.Livestock
{
    public class GetLivestockBreedList : LivestockBreedQueries, IRequest<Tuple<long, ICollection<LivestockBreedModel?>>>
    {
        public class Handler : IRequestHandler<GetLivestockBreedList, Tuple<long, ICollection<LivestockBreedModel?>>>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<Tuple<long, ICollection<LivestockBreedModel?>>> Handle(GetLivestockBreedList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery(_context);

                return new Tuple<long, ICollection<LivestockBreedModel?>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => LivestockBreedModel.Create(f)).ToListAsync(cancellationToken)
                    );
            }
        }
    }
}
