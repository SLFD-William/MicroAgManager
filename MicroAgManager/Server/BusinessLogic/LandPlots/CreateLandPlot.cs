﻿using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.LandPlots
{
    public class CreateLandPlot : BaseCommand, ICreateCommand
    {

        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public LandPlotModel LandPlot { get; set; }
        public class Handler : BaseCommandHandler<CreateLandPlot>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
            {
            }

            public override async Task<long> Handle(CreateLandPlot request, CancellationToken cancellationToken)
            {
                var plot = new Domain.Entity.LandPlot(request.ModifiedBy, request.TenantId);
                plot = request.LandPlot.MapToEntity(plot);
                plot.FarmLocation = _context.Farms.Find(plot.FarmLocationId);

                plot.ModifiedOn = plot.Created = DateTime.Now;
                plot.ModifiedBy = plot.CreatedBy = request.ModifiedBy;
                plot.TenantId = request.TenantId;

                _context.Plots.Add(plot);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex) {Console.WriteLine(ex.ToString());}

                //await _mediator.Publish(new NotificationMessage
                //{
                //    To = request.TenantId.ToString(),
                //    Body = $"{nameof(LandPlotModel)} {farm.Id}",
                //    From = request.ModifiedBy.ToString(),
                //    Subject = "Create"
                //}, cancellationToken);
                return plot.Id;
            }

        }

    }
}
