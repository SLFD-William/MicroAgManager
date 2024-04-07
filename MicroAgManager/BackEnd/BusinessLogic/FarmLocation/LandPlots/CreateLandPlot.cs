using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.FarmLocation.LandPlots
{
    public class CreateLandPlot : BaseCommand, ICreateCommand
    {

        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public LandPlotModel LandPlot { get; set; }
        public class Handler : BaseCommandHandler<CreateLandPlot>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(CreateLandPlot request, CancellationToken cancellationToken)
            {
                var plot = request.LandPlot.Map(new LandPlot(request.ModifiedBy, request.TenantId)) as LandPlot ;
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    plot.FarmLocation = context.Farms.Find(plot.FarmLocationId);

                    context.Plots.Add(plot);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new ModifiedEntityPushNotification (plot.TenantId, LandPlotModel.Create(plot).GetJsonString(), nameof(LandPlotModel)    , plot.ModifiedOn), cancellationToken);
                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Create Land Plot"); }
                }
                    return plot.Id;
            }

        }

    }
}
