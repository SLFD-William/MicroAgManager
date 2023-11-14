using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Breeds
{
    public class GetLivestockBreedList : LivestockBreedQueries, IRequest<LivestockBreedDto>
    {
        public class Handler : BaseRequestHandler<GetLivestockBreedList>, IRequestHandler<GetLivestockBreedList, LivestockBreedDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<LivestockBreedDto> Handle(GetLivestockBreedList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.LivestockBreed>(context);
                    return new LivestockBreedDto(await query.LongCountAsync(cancellationToken), await query.Select(f => LivestockBreedModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
