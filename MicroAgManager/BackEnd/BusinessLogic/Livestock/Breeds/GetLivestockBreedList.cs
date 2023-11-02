using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Breeds
{
    public class GetLivestockBreedList : LivestockBreedQueries, IRequest<Tuple<long, ICollection<LivestockBreedModel?>>>
    {
        public class Handler : BaseRequestHandler<GetLivestockBreedList>, IRequestHandler<GetLivestockBreedList, Tuple<long, ICollection<LivestockBreedModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<LivestockBreedModel?>>> Handle(GetLivestockBreedList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.LivestockBreed>(_context);

                return new Tuple<long, ICollection<LivestockBreedModel?>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => LivestockBreedModel.Create(f)).ToListAsync(cancellationToken)
                    );
            }
        }
    }
}
