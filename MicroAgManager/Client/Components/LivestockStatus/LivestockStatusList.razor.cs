using Domain.Models;
using FrontEnd.Components.Shared.Sortable;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockStatus
{
    public partial class LivestockStatusList:ComponentBase, IAsyncDisposable
    {
        protected ListTemplate<LivestockStatusModel> _listComponent;
        [Inject] ApplicationStateProvider app { get; set; }
        [CascadingParameter] LivestockTypeModel livestockType { get; set; }
        [Parameter] public IEnumerable<LivestockStatusModel>? Items { get; set; }
        protected async override Task OnInitializedAsync()
        {

            app.dbSynchonizer.OnUpdate += DbSync_OnUpdate;
            await FreshenData();
        }

        private void DbSync_OnUpdate() => Task.Run(FreshenData);

        private async Task FreshenData()
        {
            if (_listComponent is null) return;
            
            if (Items is null)
                Items = (await app.dbContext.LivestockStatuses.Where(f => f.LivestockTypeId == livestockType.Id).OrderBy(f => f.ModifiedOn).ToListAsync()).AsEnumerable();

            _listComponent.Update();
            StateHasChanged();
        }
        public ValueTask DisposeAsync()
        {
            app.dbSynchonizer.OnUpdate -= DbSync_OnUpdate;
            return ValueTask.CompletedTask;
        }
    }
}
