using Domain.Models;
using FrontEnd.Components.LivestockBreed;
using FrontEnd.Components.LivestockStatus;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockAnimal
{
    public partial class LivestockAnimalSubTabs : DataComponent
    {
        [CascadingParameter] public LivestockAnimalModel? LivestockAnimal { get; set; }
        [Parameter] public long? livestockAnimalId { get; set; }
        private LivestockAnimalModel livestockAnimal { get; set; } = new LivestockAnimalModel();
        protected TabControl _tabControl;
        protected TabPage _breedsTab;
        protected TabPage _statusTab;

        private LivestockStatusEditor? _livestockStatusEditor;
        protected LivestockStatusList _livestockStatusList;

        private LivestockBreedEditor? _livestockBreedEditor;
        protected LivestockBreedList _livestockBreedList;

        public override async Task FreshenData()
        {

            if (LivestockAnimal is not null)
                livestockAnimal = await app.dbContext.LivestockAnimals.FindAsync(LivestockAnimal.Id) ?? new LivestockAnimalModel();
            else
            {
                var query = app.dbContext?.LivestockAnimals.AsQueryable();
                if (livestockAnimalId.HasValue && livestockAnimalId > 0)
                    query = query.Where(f => f.Id == livestockAnimalId);
                livestockAnimal = await query.OrderBy(f => f.Id).SingleOrDefaultAsync() ?? new LivestockAnimalModel();
            }
            if (_livestockStatusList is not null)
                await _livestockStatusList.FreshenData();

            if (_livestockBreedList is not null)
                await _livestockBreedList.FreshenData();

        }
        private async Task LivestockStatusUpdated(LivestockStatusModel args)
        {
            if (args.Id > 0)
                while (!app.dbContext.LivestockStatuses.Any(t => t.Id == args.Id))
                    await Task.Delay(100);

            await FreshenData();
        }
        private async Task LivestockBreedUpdated(LivestockBreedModel args)
        {
            if (args.Id > 0)
                while (!app.dbContext.LivestockBreeds.Any(t => t.Id == args.Id))
                    await Task.Delay(100);

            await FreshenData();
        }
    }
}

