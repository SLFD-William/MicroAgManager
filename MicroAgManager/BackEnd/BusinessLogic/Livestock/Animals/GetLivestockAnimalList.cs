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
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<LivestockAnimalDto> Handle(GetLivestockAnimalList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.LivestockAnimal>(_context);

                return new LivestockAnimalDto
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => LivestockAnimalModel.Create(f)).ToListAsync(cancellationToken)
                    );
            }
        }
    }
}
