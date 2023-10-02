using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Registrar
{
    public class GetRegistrarList : RegistrarQueries, IRequest<Tuple<long, ICollection<RegistrarModel>>>
    {
        public class Handler : BaseRequestHandler<GetRegistrarList>, IRequestHandler<GetRegistrarList, Tuple<long, ICollection<RegistrarModel>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<RegistrarModel>>> Handle(GetRegistrarList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.Registrar>(_context);

                return new Tuple<long, ICollection<RegistrarModel>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => RegistrarModel.Create(f)).ToListAsync(cancellationToken)
                );
            }
        }
    }
}
