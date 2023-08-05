using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.LandPlots
{
    public class CreateLandPlot : BaseCommand, ICreateCommand
    {

        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public LandPlotModel LandPlot { get; set; }
        public class Handler : BaseCommandHandler<CreateLandPlot>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(CreateLandPlot request, CancellationToken cancellationToken)
            {
                var plot = request.LandPlot.MapToEntity(new Domain.Entity.LandPlot(request.ModifiedBy, request.TenantId));
                plot.FarmLocation = _context.Farms.Find(plot.FarmLocationId);

                _context.Plots.Add(plot);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(plot.Id.ToString(), plot.GetType().Name, "Created", plot.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Create Land Plot"); }
                return plot.Id;
            }

        }

    }
}
