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
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async override Task<long> Handle(UpdateUnit request, CancellationToken cancellationToken)
            {
                var unit = await _context.Units.FindAsync(request.Unit.Id);
                unit = request.Unit.MapToEntity(unit);
                _context.Units.Update(unit);
                await _context.SaveChangesAsync(cancellationToken);
                return unit.Id;
            }
        }
    }
}
