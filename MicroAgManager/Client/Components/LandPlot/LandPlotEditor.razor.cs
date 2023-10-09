﻿using BackEnd.BusinessLogic.FarmLocation.LandPlots;
using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;


namespace FrontEnd.Components.LandPlot
{
    public partial class LandPlotEditor : DataComponent
    {
        [CascadingParameter] FarmLocationModel Farm { get; set; }
        [CascadingParameter] public LandPlotModel Plot { get; set; }
        [Parameter] public long? landPlotId { get; set; }
        [Parameter] public long? parentPlotId { get; set; }

        LandPlotModel plot { get; set; }
       
        private ValidatedForm _validatedForm;
        private async void Cancel()
        {
            editContext = new EditContext(plot);
            await Cancelled.InvokeAsync();
            StateHasChanged();
        }
        public async Task OnSubmit()
        {
            if (plot.Id <= 0)
                plot.Id = await app.api.ProcessCommand<LandPlotModel, CreateLandPlot>("api/CreateLandPlot", new CreateLandPlot { LandPlot = plot });
            else
                plot.Id = await app.api.ProcessCommand<LandPlotModel, UpdateLandPlot>("api/UpdateLandPlot", new UpdateLandPlot { LandPlot = plot });

            if (plot.Id <= 0)
                throw new Exception("Unable to save plot");
            
            editContext = new EditContext(plot);
            await Submitted.InvokeAsync(plot);
            StateHasChanged();
        }
        public override async Task FreshenData()
        {
            if (Farm is null) return;
            if (Plot is not null)
            {
                plot = Plot;
                editContext = new EditContext(plot);
                StateHasChanged();
                return;
            }
            var query = app.dbContext.LandPlots.Where(f => f.FarmLocationId == Farm.Id).AsQueryable();
            if (parentPlotId.HasValue && parentPlotId > 0)
                query = query.Where(f => f.ParentPlotId == parentPlotId);
            if (landPlotId.HasValue && landPlotId > 0)
                query = query.Where(f => f.Id == landPlotId);
            plot = await query.OrderBy(f => f.Id).FirstOrDefaultAsync() ?? new LandPlotModel { FarmLocationId= Farm.Id };
            editContext = new EditContext(plot);
            StateHasChanged();
        }
    }
}
