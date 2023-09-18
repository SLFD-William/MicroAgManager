using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.LivestockAnimal
{
    public partial class LivestockAnimalList : DataComponent
    {
        [CascadingParameter] FarmLocationModel farm { get; set; }

        public TableTemplate<LivestockAnimalRow> _listComponent;
        [Parameter] public IEnumerable<LivestockAnimalRow> Items { get; set; } = new List<LivestockAnimalRow>();

        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<LivestockAnimalModel>? AnimalSelected { get; set; }
        protected override void OnInitialized()
        {
            if (!app.RowDetailsShowing.ContainsKey("LivestockAnimalList"))
                app.RowDetailsShowing.Add("LivestockAnimalList", new List<object>());
        }
        private LivestockAnimalModel? _editAnimal;
        private LivestockAnimalEditor? _animalEditor;
        private void EditAnimal(long id)
        {
            _editAnimal = id>0 ? app.dbContext.LivestockAnimals.Find(id): new LivestockAnimalModel();
            StateHasChanged();
        }

        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                AnimalSelected?.Invoke(app.dbContext.LivestockAnimals.Find(_listComponent.SelectedItems.First().Id));
        }
        public override async Task FreshenData()
        {
            if (_listComponent is null) return;
            
            if (Items is null)
                Items = app.dbContext.LivestockAnimals.OrderBy(f => f.Name).Select(f=> LivestockAnimalRow.Create(app,f)).AsEnumerable() ?? new List<LivestockAnimalRow>();
            
            _listComponent.Update();
        }
        private async Task EditCancelled()
        {
            _editAnimal = null;
            await FreshenData();
        }
        private async Task AnimalUpdated(LivestockAnimalModel args)
        {
            if (args.Id > 0)
                while (!app.dbContext.LivestockAnimals.Any(t => t.Id == args.Id))
                    await Task.Delay(100);

            _editAnimal = null;
            await FreshenData();
        }
    }
}
