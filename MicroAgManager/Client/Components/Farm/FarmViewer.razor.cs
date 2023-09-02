﻿using Domain.Models;
using FrontEnd.Components.LandPlot;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.Farm
{
    public partial class FarmViewer :DataComponent
    {
        [Inject] NavigationManager navigationManager { get; set; }
        [CascadingParameter] public FarmLocationModel? FarmLocation { get; set; }
        [Parameter] public long? farmId { get; set; }

        private LandPlotEditor? landPlotEditor;
        private LivestockAnimalEditor? livestockAnimalEditor;
        private FarmLocationModel farm { get; set; } = new FarmLocationModel();
        protected TabControl _tabControl;
        protected TabPage _plotTab;
        protected TabPage _livestockTab;
        protected TabPage _dutyTab;
        private string GetPlotCount(string plotUsage)
        { 
            var count = app.dbContext.LandPlots.Count(p => p.FarmLocationId == farm.Id && p.Usage == plotUsage);
            return count>0 ? $"<p>{plotUsage} {count}</p>":string.Empty;
        }
        public override async Task FreshenData()
        {
            if (FarmLocation is not null)
            {
                farm = FarmLocation;
                StateHasChanged();
                return;
            }
            var query = app.dbContext?.Farms.AsQueryable();
            if (farmId.HasValue && farmId > 0)
                query = query.Where(f => f.Id == farmId);
            farm = await query.OrderBy(f => f.Id).SingleOrDefaultAsync() ?? new FarmLocationModel();
            StateHasChanged();
        }
        private async Task LandPlotUpdated(LandPlotModel args)
        {
            if (args.Id > 0)
                while (!app.dbContext.LandPlots.Any(t => t.Id == args.Id))
                    await Task.Delay(100);

            await FreshenData();
        }
    }
}
