using Domain.Models;
using FrontEnd.Components.LivestockBreed;
using FrontEnd.Components.LivestockStatus;
using FrontEnd.Components.Milestone;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockAnimal
{
    public partial class LivestockAnimalSubTabs : DataComponent
    {
        [CascadingParameter] public LivestockAnimalSummary? LivestockAnimal { get; set; }
        [Parameter] public long? livestockAnimalId { get; set; }
        private LivestockAnimalModel livestockAnimal { get; set; } = new LivestockAnimalModel();
        protected TabControl _tabControl;
        protected TabPage _breedsTab;
        protected LivestockBreedList _livestockBreedList;
        protected TabPage _statusTab;
        protected LivestockStatusList _livestockStatusList;
        protected TabPage _milestoneTab;
        protected MilestoneList _milestoneList;

        protected async override Task OnInitializedAsync()
        {
            _tabControl?.ActivatePage(app.SelectedTabs["LivestockAnimalSubTabs"] ?? _tabControl?.ActivePage ?? _breedsTab);
            await FreshenData();
        }
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

            if (_milestoneList is not null)
                await _milestoneList.FreshenData();

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
        private async Task MilestoneUpdated(MilestoneModel args)
        {
            if (args.Id > 0)
                while (!app.dbContext.Milestones.Any(t => t.Id == args.Id))
                    await Task.Delay(100);

            await FreshenData();
        }
    }
}

