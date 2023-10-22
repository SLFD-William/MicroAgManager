using Domain.Models;
using FrontEnd.Components.Measurement;
using FrontEnd.Components.Registration;
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
        private LivestockBreedModel breed { get; set; }

        protected TabPage _registrationTab;
        protected RegistrationList _registrationList;
        protected TabPage _treatmentRecordTab;
        protected TreatmentRecordList _treatmentRecordList;
        protected TabPage _measurementTab;
        protected MeasurementList _measurementList;
        protected TabControl _tabControl;

        protected TabPage _progenyTab;
        protected TabPage _ancestryTab;
        public override async Task FreshenData()
        {
            livestock = Livestock is not null ? Livestock :
               await app.dbContext.Livestocks.FindAsync(livestockId.Value);

            if(livestock is not null)
                breed = await app.dbContext.LivestockBreeds.FindAsync(livestock.LivestockBreedId);

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
