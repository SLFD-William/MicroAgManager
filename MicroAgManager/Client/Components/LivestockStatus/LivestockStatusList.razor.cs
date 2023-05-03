using Domain.Models;
using FrontEnd.Components.Shared.Sortable;
using FrontEnd.Data;
using FrontEnd.Persistence;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockStatus
{
    public partial class LivestockStatusList:ComponentBase, IAsyncDisposable
    {
        protected ListTemplate<LivestockStatusModel> _listComponent;
        [CascadingParameter] LivestockTypeModel livestockType { get; set; }
        [CascadingParameter] DataSynchronizer dbSync { get; set; }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        [Parameter] public IEnumerable<LivestockStatusModel>? Items { get; set; }
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
                Items = (await dbContext.LivestockStatuses.Where(f => f.LivestockTypeId == livestockType.Id).OrderBy(f => f.ModifiedOn).ToListAsync()).AsEnumerable();

            _listComponent.Update();
            StateHasChanged();
        }
        public ValueTask DisposeAsync()
        {
            dbSync.OnUpdate -= DbSync_OnUpdate;
            return ValueTask.CompletedTask;
        }
    }
}
