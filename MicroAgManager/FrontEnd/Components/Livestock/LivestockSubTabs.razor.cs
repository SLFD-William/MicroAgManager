using Domain.Models;
using FrontEnd.Components.Farm;
using FrontEnd.Components.ScheduledDuty;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.Livestock
{
    public partial class LivestockSubTabs: DataComponent<LivestockModel>
    {
        [CascadingParameter] public LivestockModel? Livestock { get; set; }
        [Parameter] public long? livestockId { get; set; }
        private LivestockModel livestock { get; set; } = new LivestockModel();

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
               await app.dbContext.Livestocks
                .Include(p => p.Status)
                .Include(p => p.Breed).ThenInclude(p => p.Animal)
                .Include(p => p.Mother).Include(p => p.Father)
                .FirstOrDefaultAsync(i => i.Id == livestockId); 

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
