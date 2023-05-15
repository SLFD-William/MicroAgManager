using Domain.Models;
using FrontEnd.Components.Shared.Sortable;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.LandPlot
{
    public partial class LandPlotList:ComponentBase, IAsyncDisposable
    {
        [CascadingParameter] ApplicationStateProvider app { get; set; }
        [CascadingParameter] FarmLocationModel farm { get; set; }

        public TableTemplate<LandPlotModel> _listComponent;
        [Parameter] public IEnumerable<LandPlotModel>? Items { get; set; }
        [Parameter] public bool Selectable { get; set; } = false;
        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<LandPlotModel>? PlotSelected { get; set; }
        protected async override Task OnInitializedAsync()
        {
            app.dbSynchonizer.OnUpdate += DbSync_OnUpdate;
            await FreshenData();
        }

        private void TableItemSelected()
        { 
            if(_listComponent.SelectedItems.Count() > 0)
                PlotSelected?.Invoke(_listComponent.SelectedItems.First());
        }
        private void DbSync_OnUpdate() => Task.Run(FreshenData);

        private async Task FreshenData()
        {
            if (_listComponent is null) return;

            if (Items is null)
                Items = app.dbContext.LandPlots.Where(f => f.FarmLocationId == farm.Id).OrderBy(f => f.ModifiedOn).AsEnumerable();
           
            StateHasChanged();
            _listComponent.Update();
        }
        public ValueTask DisposeAsync()
        {
            app.dbSynchonizer.OnUpdate -= DbSync_OnUpdate;
            return ValueTask.CompletedTask;
        }
    }
}
