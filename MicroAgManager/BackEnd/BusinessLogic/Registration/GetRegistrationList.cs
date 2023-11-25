using BackEnd.Abstracts;
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
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<RegistrationDto> Handle(GetRegistrationList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.Registration>(context);

                    return new RegistrationDto(await query.LongCountAsync(cancellationToken), await query.Select(f => RegistrationModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
