using Domain.Models;
using FrontEnd.Components.Farm;
using FrontEnd.Components.ScheduledDuty;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.Livestock
{
    public partial class LivestockSubTabs: DataComponent<LivestockModel>
    {
        [CascadingParameter] public LivestockModel? Livestock { get; set; }
        [Parameter] public long? livestockId { get; set; }
        private LivestockModel livestock { get; set; } = new LivestockModel();
        private LivestockModel livestockMom { get; set; } = new LivestockModel();
        private LivestockModel livestockDad { get; set; } = new LivestockModel();
        private LivestockBreedModel breed { get; set; }

        protected TabPage _closeTab;
        protected TabPage _registrationTab;
        protected ScheduledDutyList _registrationList;
        protected TabPage _treatmentRecordTab;
        protected ScheduledDutyList _treatmentRecordList;
        protected TabPage _measurementTab;
        protected ScheduledDutyList _measurementList;
        protected TabControl _tabControl;

        protected TabPage _progenyTab;
        protected TabPage _ancestryTab;
        public override async Task FreshenData()
        {
            livestock = Livestock is not null ? Livestock :
               await app.dbContext.Livestocks.FindAsync(livestockId.Value);

            breed = await app.dbContext.LivestockBreeds.FindAsync(livestock?.LivestockBreedId);
            livestockMom = await app.dbContext.Livestocks.FindAsync(livestock?.MotherId);
            livestockDad = await app.dbContext.Livestocks.FindAsync(livestock?.FatherId);

            if (_registrationList is not null)
                await _registrationList.FreshenData();
            if (_treatmentRecordList is not null)
                await _treatmentRecordList.FreshenData();
            if (_measurementList is not null)
                await _measurementList.FreshenData();
        }
        protected override void OnParametersSet()
        {
            app.SelectedTabs.TryGetValue(nameof(LivestockSubTabs), out var selected);
            _tabControl?.ActivatePage(selected ?? _tabControl?.ActivePage ?? _closeTab);
        }
       
    }
}
