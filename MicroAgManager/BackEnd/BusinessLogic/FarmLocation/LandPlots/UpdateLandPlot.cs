﻿using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
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
                    var plot = await context.Plots.FirstAsync(f => f.TenantId == request.TenantId && f.Id == request.LandPlot.Id);
                    plot = request.LandPlot.Map(plot) as Domain.Entity.LandPlot;
                    plot.ModifiedBy = request.ModifiedBy;

                    await context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new ModifiedEntityPushNotification (plot.TenantId, LandPlotModel.Create(plot).GetJsonString(), nameof(LandPlotModel), plot.ModifiedOn), cancellationToken);

                    return plot.Id;
                }
            }
        }
    }
}
