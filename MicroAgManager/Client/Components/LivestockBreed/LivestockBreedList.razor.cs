using Domain.Models;
using FrontEnd.Components.Shared.Sortable;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.LivestockBreed
{
    public partial class LivestockBreedList:ComponentBase, IAsyncDisposable
    {
        public ListTemplate<LivestockBreedModel> _listComponent;
        [Inject] ApplicationStateProvider app { get; set; }
        [CascadingParameter] LivestockTypeModel livestockType { get; set; }
        [Parameter] public IEnumerable<LivestockBreedModel>? Items { get; set; }
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
                Items = app.dbContext.LivestockBreeds.Where(f => f.LivestockTypeId == livestockType.Id).OrderBy(f => f.ModifiedOn).AsEnumerable();

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
