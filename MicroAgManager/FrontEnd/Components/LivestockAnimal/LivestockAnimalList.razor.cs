using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.LivestockAnimal
{
    public partial class LivestockAnimalList : DataComponent<LivestockAnimalModel>
    {

        public TableTemplate<LivestockAnimalSummary> _listComponent;
        [Parameter] public IEnumerable<LivestockAnimalSummary> Items { get; set; } = new List<LivestockAnimalSummary>();

        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<LivestockAnimalModel>? AnimalSelected { get; set; }
        private LivestockAnimalModel? _editAnimal;
        private LivestockAnimalEditor? _animalEditor;
        private async Task<LivestockAnimalModel?> FindAnimal(long Id) => await app.dbContext.LivestockAnimals.FindAsync(Id);
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                AnimalSelected?.Invoke(Task.Run(async () => await FindAnimal(_listComponent.SelectedItems.First().Id)).Result);
        }
        private async Task EditAnimal(long id)
        {
            _editAnimal = id > 0 ? await FindAnimal(id) : new LivestockAnimalModel();
            StateHasChanged();
        }
        private async Task EditCancelled()
        {
            _editAnimal = null;
            StateHasChanged(); 
        }
        private async Task AnimalUpdated(object args)
        {
            var model = args as LivestockAnimalModel;
            if (model?.Id > 0)
                while (!app.dbContext.LivestockAnimals.Any(t => t.Id == model.Id))
                    await Task.Delay(100);
            _editAnimal = null;
            await Submitted.InvokeAsync(await FindAnimal(model.Id));
        }
        protected override void OnInitialized()
        {
            if (!app.RowDetailsShowing.ContainsKey("LivestockAnimalList"))
                app.RowDetailsShowing.Add("LivestockAnimalList", new List<object>());
        }
        
        

        
        public override async Task FreshenData()
        {
            if (_listComponent is null) return;
            while (!app.dbContext.LivestockAnimals.Any())
                await Task.Delay(100);
            if (Items is null)
                Items = app.dbContext.LivestockAnimals.OrderBy(f => f.Name).Select(f=> new LivestockAnimalSummary(f,app.dbContext)).AsEnumerable() ?? new List<LivestockAnimalSummary>();
            
            _listComponent.Update();
        }

        
    }
}
