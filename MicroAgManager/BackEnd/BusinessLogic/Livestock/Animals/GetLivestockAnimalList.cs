using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Animals
{
    public class GetLivestockAnimalList : LivestockAnimalQueries, IRequest<Tuple<long, ICollection<LivestockAnimalModel?>>>
    {
        public class Handler : BaseRequestHandler<GetLivestockAnimalList>, IRequestHandler<GetLivestockAnimalList, Tuple<long, ICollection<LivestockAnimalModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<LivestockAnimalModel?>>> Handle(GetLivestockAnimalList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.LivestockAnimal>(_context);

                return new Tuple<long, ICollection<LivestockAnimalModel?>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => LivestockAnimalModel.Create(f)).ToListAsync(cancellationToken)
                    );
            }
        }
    }
}
