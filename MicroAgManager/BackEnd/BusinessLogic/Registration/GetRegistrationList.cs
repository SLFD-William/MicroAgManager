using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Registration
{
    public class GetRegistrationList:RegistrationQueries, IRequest<RegistrationDto>
    {
        public class Handler : BaseRequestHandler<GetRegistrationList>, IRequestHandler<GetRegistrationList, RegistrationDto>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<RegistrationDto> Handle(GetRegistrationList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.Registration>(_context);

                return new RegistrationDto
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => RegistrationModel.Create(f)).ToListAsync(cancellationToken)
                );
            }
        }
    }
}
