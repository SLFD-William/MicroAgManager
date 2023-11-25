using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.ScheduledDuty
{
    public partial class ScheduledDutyCommand : DataComponent<ScheduledDutyModel>
    {
        [CascadingParameter] public ScheduledDutyModel ScheduledDuty { get; set; }
        private bool _modalOpen = false;
        private ScheduledDutyEditor? _scheduledDutyEditor;
        public override async Task FreshenData()=>await InvokeAsync(StateHasChanged);
        private void ShowModal()
        {
            _modalOpen = true;
            StateHasChanged();
        }
        private async Task EditCancelled()
        {
            _modalOpen = false;
            await Cancelled.InvokeAsync(); 
            StateHasChanged();
        }
        private async Task ScheduledDutyUpdated(object args)
        {
            _modalOpen = false;
            await Submitted.InvokeAsync(args);
            StateHasChanged();
        }
    }
}
