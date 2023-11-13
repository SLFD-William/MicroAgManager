using BackEnd.Abstracts;
using Domain.Interfaces;
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
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<RegistrarDto> Handle(GetRegistrarList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.Registrar>(_context);

                return new RegistrarDto
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => RegistrarModel.Create(f)).ToListAsync(cancellationToken)
                );
            }
        }
    }
}
