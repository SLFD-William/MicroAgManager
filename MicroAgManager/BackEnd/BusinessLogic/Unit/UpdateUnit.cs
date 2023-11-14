using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Unit
{
    public class UpdateUnit:BaseCommand,IUpdateCommand
    {
        public required UnitModel Unit { get; set; }

        public class Handler:BaseCommandHandler<UpdateUnit>,IRequestHandler<UpdateUnit, long>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async override Task<long> Handle(UpdateUnit request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var unit = request.Unit.Map(await context.Units.FindAsync(request.Unit.Id)) as Domain.Entity.Unit;
                    context.Units.Update(unit);
                    await context.SaveChangesAsync(cancellationToken);
                    return unit.Id;
                }
            }
        }
    }
}
