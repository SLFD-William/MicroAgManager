using BackEnd.Abstracts;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Registrar
{
    public class GetRegistrarList : RegistrarQueries, IRequest<RegistrarDto>
    {
        public class Handler : BaseRequestHandler<GetRegistrarList>, IRequestHandler<GetRegistrarList, RegistrarDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<RegistrarDto> Handle(GetRegistrarList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.Registrar>(context);

                    return new RegistrarDto(await query.LongCountAsync(cancellationToken), await query.Select(f => RegistrarModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
