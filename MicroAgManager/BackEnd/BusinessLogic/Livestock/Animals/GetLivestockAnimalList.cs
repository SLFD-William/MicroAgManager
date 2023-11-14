using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Animals
{
    public class GetLivestockAnimalList : LivestockAnimalQueries, IRequest<LivestockAnimalDto>
    {
        public class Handler : BaseRequestHandler<GetLivestockAnimalList>, IRequestHandler<GetLivestockAnimalList, LivestockAnimalDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<LivestockAnimalDto> Handle(GetLivestockAnimalList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.LivestockAnimal>(context);

                    return new LivestockAnimalDto(await query.LongCountAsync(cancellationToken), await query.Select(f => LivestockAnimalModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
