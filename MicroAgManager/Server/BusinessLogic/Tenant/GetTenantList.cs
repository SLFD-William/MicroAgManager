using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.Tenant
{
    public class GetTenantList:TenantQueries, IRequest<Tuple<long, ICollection<TenantModel?>>>
    {
        public class Handler : IRequestHandler<GetTenantList, Tuple<long, ICollection<TenantModel?>>>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<Tuple<long, ICollection<TenantModel?>>> Handle(GetTenantList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery(_context);

                return new Tuple<long, ICollection<TenantModel?>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => TenantModel.Create(f)).ToListAsync(cancellationToken)
                    );
            }
        }
    }
}
