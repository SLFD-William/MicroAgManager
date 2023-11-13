using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockBreed
{
    public partial class LivestockBreedList:DataComponent<LivestockBreedModel>
    {
        public TableTemplate<LivestockBreedSummary> _listComponent;
        [CascadingParameter] LivestockAnimalSummary LivestockAnimal { get; set; }
        [Parameter] public IEnumerable<LivestockBreedSummary>? Items { get; set; }

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
                Items = (await app.dbContext.LivestockBreeds.Where(f => f.LivestockAnimalId == LivestockAnimal.Id)
                    .OrderBy(f => f.EntityModifiedOn).ToListAsync()).Select(b=>new LivestockBreedSummary(b,app.dbContext)).AsEnumerable();

            _listComponent.Update();
        }
        private async Task<LivestockBreedModel?> FindBreed(long Id) => await app.dbContext.LivestockBreeds.FindAsync(Id);
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                BreedSelected?.Invoke(Task.Run(async()=> await FindBreed(_listComponent.SelectedItems.First().Id)).Result);
        }
        private async Task EditBreed(long id)
        {
            _editBreed = id > 0 ? await FindBreed(id) : new LivestockBreedModel {LivestockAnimalId=LivestockAnimal.Id }; 
            StateHasChanged();
        }
        private async Task EditCancelled()
        {
            _editBreed = null;
            StateHasChanged();
        }
        private async Task BreedUpdated(object args)
        {
            var model=args as LivestockBreedModel;
            if (model?.Id > 0)
            {
                var start = DateTime.Now;
                while (!app.dbContext.LivestockBreeds.Any(t => t.Id == model.Id))
                {
                    await Task.Delay(1000);
                    if (DateTime.Now.Subtract(start).TotalSeconds > 10)
                        break;
                }
            }
            _editBreed = null;
            await Submitted.InvokeAsync(await FindBreed(model.Id));
        }
    }
}
