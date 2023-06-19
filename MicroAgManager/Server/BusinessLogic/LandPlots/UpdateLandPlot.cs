using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.LandPlots
{
    public class UpdateLandPlot : BaseCommand, IUpdateCommand
    {
        public LandPlotModel LandPlot { get; set; }

        public class Handler : BaseCommandHandler<UpdateLandPlot>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateLandPlot request, CancellationToken cancellationToken)
            {
                var farm = _context.Plots.First(f => f.TenantId == request.TenantId && f.Id == request.LandPlot.Id);
                farm = request.LandPlot.MapToEntity(farm);
                farm.ModifiedOn = DateTime.Now;
                farm.ModifiedBy = request.ModifiedBy;

                await _context.SaveChangesAsync(cancellationToken);
                await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(farm.Id.ToString(), farm.GetType().Name, "Modified", farm.ModifiedBy) }), cancellationToken);

                return farm.Id;
            }
        }
    }
}
