using Domain.Models;
using FrontEnd.Components.Measurement;
using FrontEnd.Components.Registration;
using FrontEnd.Components.ScheduledDuty;
using FrontEnd.Components.Shared;
using FrontEnd.Components.TreatmentRecord;
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
        protected override void OnInitialized() =>
            _tabControl?.ActivatePage(app.SelectedTabs[nameof(LivestockSubTabs)] ?? _tabControl?.ActivePage);

    }
}
