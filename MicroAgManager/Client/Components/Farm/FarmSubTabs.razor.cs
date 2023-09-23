using Domain.Models;
using FrontEnd.Components.LandPlot;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.Farm
{
    public partial class FarmSubTabs : DataComponent
    {
        [CascadingParameter] public FarmLocationModel? FarmLocation { get; set; }
        [Parameter] public long? farmId { get; set; }

        private LandPlotEditor? _landPlotEditor;
        protected LandPlotList _landPlotList;
        private LivestockAnimalEditor? _livestockAnimalEditor;
        protected LivestockAnimalList _livestockAnimalList;
        private FarmLocationModel farm { get; set; } = new FarmLocationModel();
        protected TabControl _tabControl;
        protected TabPage _plotTab;
        protected TabPage _livestockTab;
        protected TabPage _dutyTab;
        protected override void OnInitialized()
        {
            _tabControl?.ActivatePage(app.SelectedTabs[nameof(FarmSubTabs)] ?? _tabControl?.ActivePage ?? _plotTab);
        }
        private string GetPlotCount(string plotUsage)
        {
            var count = app.dbContext.LandPlots.Count(p => p.FarmLocationId == farm.Id && p.Usage == plotUsage);
            return count > 0 ? $"<p>{plotUsage} {count}</p>" : string.Empty;
        }
        public override async Task FreshenData()
        {
            if (FarmLocation is not null)
                farm = await app.dbContext.Farms.FindAsync(FarmLocation.Id) ?? new FarmLocationModel();
            else
            {   var query = app.dbContext?.Farms.AsQueryable();
                if (farmId.HasValue && farmId > 0)
                    query = query.Where(f => f.Id == farmId);
                farm = await query.OrderBy(f => f.Id).SingleOrDefaultAsync() ?? new FarmLocationModel();
            }
            //await _landPlotList.FreshenData();
            if(_livestockAnimalList is not null)
                await _livestockAnimalList.FreshenData();

            
        }
        private async Task LandPlotUpdated(LandPlotModel args)
        {
            if (args.Id > 0)
                while (!app.dbContext.LandPlots.Any(t => t.Id == args.Id))
                    await Task.Delay(100);

            await FreshenData();
        }
        private async Task LivestockAnimalUpdated(LivestockAnimalModel args)
        {
            if (args.Id > 0)
                while (!app.dbContext.LivestockAnimals.Any(t => t.Id == args.Id))
                    await Task.Delay(100);

            await FreshenData();
        }
    }
}
