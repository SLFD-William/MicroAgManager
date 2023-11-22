using Domain.Models;
using FrontEnd.Components.Farm;
using FrontEnd.Components.Livestock;
using FrontEnd.Components.LivestockBreed;
using FrontEnd.Components.LivestockStatus;
using FrontEnd.Components.Milestone;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.LivestockAnimal
{
    public partial class LivestockAnimalSubTabs : DataComponent<LivestockAnimalModel>
    {
        [CascadingParameter] public LivestockAnimalModel? Animal { get; set; }
        [Parameter] public long? livestockAnimalId { get; set; }
        
        protected TabControl _tabControl;
        protected TabPage _closeTab;
        protected TabPage _breedsTab;
        protected TabPage _statusTab;
        protected TabPage _milestoneTab;


        private LivestockAnimalModel animal { get; set; } = new LivestockAnimalModel();

        protected MilestoneList _milestoneList;
        protected LivestockBreedList _livestockBreedList;
        protected LivestockStatusList _livestockStatusList;
        private async Task TabUpdated()
        {
            await Submitted.InvokeAsync();
            StateHasChanged();
        }
        protected override void OnParametersSet()
        {
            app.SelectedTabs.TryGetValue(nameof(LivestockAnimalSubTabs), out var selected);
            if (_tabControl != null)
                _tabControl?.ActivatePage(selected ?? _tabControl?.ActivePage ?? _closeTab);
        }

        private IEnumerable<MilestoneSummary> milestoneSummaries =>
            app.dbContext.Milestones.Where(s => s.RecipientTypeId == animal.Id && s.RecipientType == animal.GetEntityName())
                .OrderBy(a => a.Name).Select(s => new MilestoneSummary(s, app.dbContext));

        public override async Task FreshenData()
        {
            animal=Animal is not null ? Animal : 
                await app.dbContext.LivestockAnimals.FindAsync(livestockAnimalId.Value);

            if (_livestockStatusList is not null)
                await _livestockStatusList.FreshenData();

            if (_livestockBreedList is not null)
                await _livestockBreedList.FreshenData();

            if (_milestoneList is not null)
                await _milestoneList.FreshenData();
        }
    }
}

