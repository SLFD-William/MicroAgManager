using Domain.Models;
using FrontEnd.Components.LivestockBreed;
using FrontEnd.Components.LivestockStatus;
using FrontEnd.Components.Milestone;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.LivestockAnimal
{
    public partial class LivestockAnimalSubTabs : DataComponent<LivestockAnimalModel>
    {
        [Parameter] public long? livestockAnimalId { get; set; }
        private LivestockAnimalSummary livestockAnimal { get; set; }
        protected TabControl _tabControl;
        protected TabPage _breedsTab;
        protected LivestockBreedList _livestockBreedList;
        protected TabPage _statusTab;
        protected LivestockStatusList _livestockStatusList;
        protected TabPage _milestoneTab;
        protected MilestoneList _milestoneList;
        protected async override Task OnParametersSetAsync()
        {
            _tabControl?.ActivatePage(app.SelectedTabs["LivestockAnimalSubTabs"] ?? _tabControl?.ActivePage ?? _breedsTab);
            await FreshenData();
        }
        private IEnumerable<MilestoneSummary> milestoneSummaries =>
            app.dbContext.Milestones.Where(s => s.RecipientTypeId == livestockAnimal.Id && s.RecipientType == livestockAnimal.EntityName)
                .OrderBy(a => a.Name).Select(s => new MilestoneSummary(s, app.dbContext));

        public override async Task FreshenData()
        {
            if (livestockAnimal is null && livestockAnimalId > 0)
                livestockAnimal = new LivestockAnimalSummary(app.dbContext?.LivestockAnimals.Find(livestockAnimalId), app.dbContext);

            if (_livestockStatusList is not null)
                await _livestockStatusList.FreshenData();

            if (_livestockBreedList is not null)
                await _livestockBreedList.FreshenData();

            if (_milestoneList is not null)
                await _milestoneList.FreshenData();
            StateHasChanged();
        }
        private async Task LivestockStatusUpdated(LivestockStatusModel args)
        {
            if (args.Id > 0)
            {
                var start = DateTime.Now;
                while (!app.dbContext.LivestockStatuses.Any(t => t.Id == args.Id))
                {
                    await Task.Delay(1000);
                    if (DateTime.Now.Subtract(start).TotalSeconds > 10)
                        break;
                }
            }

            await FreshenData();
        }
        private async Task LivestockBreedUpdated(LivestockBreedModel args)
        {
            if (args.Id > 0)
            {
                var start = DateTime.Now;
                while (!app.dbContext.LivestockBreeds.Any(t => t.Id == args.Id))
                {
                    await Task.Delay(1000);
                    if (DateTime.Now.Subtract(start).TotalSeconds > 10)
                        break;
                }
            }

            await FreshenData();
        }
        private async Task MilestoneUpdated(MilestoneModel args)
        {
            if (args.Id > 0)
            {
                var start = DateTime.Now;
                while (!app.dbContext.Milestones.Any(t => t.Id == args.Id))
                {
                    await Task.Delay(1000);
                    if (DateTime.Now.Subtract(start).TotalSeconds > 10)
                        break;
                }
            }
            await FreshenData();
        }
    }
}

