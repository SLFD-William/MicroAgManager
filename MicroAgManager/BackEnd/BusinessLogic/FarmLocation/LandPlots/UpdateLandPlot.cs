using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.FarmLocation.LandPlots
{
    public class UpdateLandPlot : BaseCommand, IUpdateCommand
    {
        public LandPlotModel LandPlot { get; set; }

        public class Handler : BaseCommandHandler<UpdateLandPlot>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateLandPlot request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var farm = await context.Plots.FirstAsync(f => f.TenantId == request.TenantId && f.Id == request.LandPlot.Id);
                    farm = request.LandPlot.Map(farm) as Domain.Entity.LandPlot;
                    farm.ModifiedBy = request.ModifiedBy;

                    await context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(farm.Id.ToString(), farm.GetType().Name, "Modified", farm.ModifiedBy) }), cancellationToken);

                    return farm.Id;
                }
            }
        }
    }
}
