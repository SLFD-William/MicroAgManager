using Domain.Models;
using FrontEnd.Components.Shared.Sortable;
using FrontEnd.Data;
using FrontEnd.Persistence;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LandPlot
{
    public partial class LandPlotList:ComponentBase, IAsyncDisposable
    {
        public ListTemplate<LandPlotModel> _listComponent;
        [CascadingParameter] FarmLocationModel farm { get; set; }
        [CascadingParameter] DataSynchronizer dbSync { get; set; }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        [Parameter] public IEnumerable<LandPlotModel>? Items { get; set; }
        protected async override Task OnInitializedAsync()
        {
            
            dbSync.OnUpdate += DbSync_OnUpdate;
            await FreshenData();
        }

        private void DbSync_OnUpdate() => Task.Run(FreshenData);

        private async Task FreshenData()
        {
            if (_listComponent is null) return;
            if (dbContext is null)
                dbContext = await dbSync.GetPreparedDbContextAsync();

            if (Items is null)
                Items = dbContext.LandPlots.Where(f => f.FarmLocationId == farm.Id).OrderBy(f => f.ModifiedOn).AsEnumerable();
           
            StateHasChanged();
            _listComponent.Update();
        }
        public ValueTask DisposeAsync()
        {
            dbSync.OnUpdate -= DbSync_OnUpdate;
            return ValueTask.CompletedTask;
        }
    }
}
