using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockBreed
{
    public partial class LivestockBreedList:DataComponent
    {
        public TableTemplate<LivestockBreedModel> _listComponent;
        [CascadingParameter] LivestockAnimalModel LivestockAnimal { get; set; }
        [Parameter] public IEnumerable<LivestockBreedModel>? Items { get; set; }

        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<LivestockBreedModel>? BreedSelected { get; set; }

        private LivestockBreedModel? _editBreed;
        private LivestockBreedEditor? _breedEditor;
        protected override void OnInitialized()
        {
            if (!app.RowDetailsShowing.ContainsKey("LivestockBreedList"))
                app.RowDetailsShowing.Add("LivestockBreedList", new List<object>());
        }
        public override async Task FreshenData()
        {
            if (_listComponent is null) return;

            if (Items is null)
                Items = (await app.dbContext.LivestockBreeds.Where(f => f.LivestockAnimalId == LivestockAnimal.Id).OrderBy(f => f.ModifiedOn).ToListAsync()).AsEnumerable();

            _listComponent.Update();
        }
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                BreedSelected?.Invoke(_listComponent.SelectedItems.First());
        }
        private void EditBreed(long id)
        {
            _editBreed = id > 0 ? Items.First(p => p.Id == id) : new LivestockBreedModel {LivestockAnimalId=LivestockAnimal.Id }; 
            StateHasChanged();
        }
        private async Task EditCancelled()
        {
            _editBreed = null;
            await FreshenData();
        }
        private async Task BreedUpdated(LivestockBreedModel args)
        {
            if (args.Id > 0)
                while (!app.dbContext.LivestockBreeds.Any(t => t.Id == args.Id))
                    await Task.Delay(100);

            _editBreed = null;
            await FreshenData();
        }
    }
}
