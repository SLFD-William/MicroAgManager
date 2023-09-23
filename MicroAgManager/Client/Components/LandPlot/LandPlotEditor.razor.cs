﻿using BackEnd.BusinessLogic.LandPlots;
using Domain.Models;
using FrontEnd.Components.Farm;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;


namespace FrontEnd.Components.LandPlot
{
    public partial class LandPlotEditor : DataComponent
    {
        [CascadingParameter] FarmLocationSummary farm { get; set; }
        [Parameter] public long? landPlotId { get; set; }
        [Parameter] public long? parentPlotId { get; set; }
        [Parameter] public bool showUpdateCancelButtons { get; set; }
        [Parameter] public EditContext editContext { get; set; }
        LandPlotModel plot { get; set; }
        protected override async Task OnInitializedAsync() => await FreshenData();
        [Parameter] public EventCallback<LandPlotModel> Submitted { get; set; }
        [Parameter] public EventCallback Cancelled { get; set; }
        private void ApplyRelatedIds()
        {
            if (farm is not null) plot.FarmLocationId = farm.Id;
            if (parentPlotId.HasValue && parentPlotId > 0) plot.ParentPlotId = parentPlotId;

        }
        private async void Cancel()
        {
            editContext = new EditContext(plot);
            await Cancelled.InvokeAsync();
            StateHasChanged();
        }
        private bool _submitting =false;
        public async Task OnSubmit()
        {
            _submitting = true;
            var id = plot.Id;
            if (id <= 0)
                id = await app.api.ProcessCommand<LandPlotModel, CreateLandPlot>("api/CreateLandPlot", new CreateLandPlot { LandPlot = plot });
            else
                id = await app.api.ProcessCommand<LandPlotModel, UpdateLandPlot>("api/UpdateLandPlot", new UpdateLandPlot { LandPlot = plot });

            if (id <= 0)
                throw new Exception("Unable to save plot");
            plot.Id = id;
            editContext = new EditContext(plot);
            editContext.MarkAsUnmodified();
            await Submitted.InvokeAsync(plot);
            //_submitting = false;
            //if (createOnly)
            //{
            //    plot = new LandPlotModel();
            //    ApplyRelatedIds();
            //    editContext = new EditContext(plot);
            //    editContext.MarkAsUnmodified();
            //    await Submitted.InvokeAsync(plot);
            //}

            StateHasChanged();
        }
        public override async Task FreshenData()
        {
            var query = app.dbContext.LandPlots.AsQueryable();
            if (farm is not null) query = query.Where(f => f.FarmLocationId == farm.Id);
            if (parentPlotId.HasValue && parentPlotId > 0)
                query = query.Where(f => f.ParentPlotId == parentPlotId);
            if (landPlotId.HasValue && landPlotId > 0)
                query = query.Where(f => f.Id == landPlotId);
            plot = new LandPlotModel();
            plot = await query.OrderBy(f => f.Id).FirstOrDefaultAsync() ?? new LandPlotModel();
            ApplyRelatedIds();
            editContext = new EditContext(plot);
            StateHasChanged();
        }
    }
}
