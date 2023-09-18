using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockStatus
{
    public partial class LivestockStatusList:DataComponent
    {
        protected TableTemplate<LivestockStatusModel> _listComponent;
        [CascadingParameter] LivestockAnimalModel LivestockAnimal { get; set; }
        [Parameter] public IEnumerable<LivestockStatusModel>? Items { get; set; }
        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<LivestockStatusModel>? StatusSelected { get; set; }

        private LivestockStatusModel? _editStatus;
        private LivestockStatusEditor? _statusEditor;
        public override async Task FreshenData()
        {
            if (_listComponent is null) return;
            
            if (Items is null)
                Items = (await app.dbContext.LivestockStatuses.Where(f => f.LivestockAnimalId == LivestockAnimal.Id).OrderBy(f => f.ModifiedOn).ToListAsync()).AsEnumerable();

            _listComponent.Update();
        }
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                StatusSelected?.Invoke(_listComponent.SelectedItems.First());
        }
        private void EditStatus(long id)
        {
            _editStatus = Items.First(p => p.Id == id);
            StateHasChanged();
        }
        private async Task EditCancelled()
        {
            _editStatus = null;
            await FreshenData();
        }
        private async Task StatusUpdated(LivestockStatusModel args)
        {
            if (args.Id > 0)
                while (!app.dbContext.LivestockStatuses.Any(t => t.Id == args.Id))
                    await Task.Delay(100);

            _editStatus = null;
            await FreshenData();
        }
    }
}
