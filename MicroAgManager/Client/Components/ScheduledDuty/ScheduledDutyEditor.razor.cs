using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.ScheduledDuty;
using FrontEnd.Services;

namespace FrontEnd.Components.ScheduledDuty
{
    public partial class ScheduledDutyEditor : DataComponent
    {
        [Inject] FrontEndAuthenticationStateProvider _auth { get; set; }
        [CascadingParameter] public ScheduledDutyModel ScheduledDuty { get; set; }
        [Parameter] public bool showUpdateCancelButtons { get; set; }
        [Parameter] public EditContext editContext { get; set; }
        [Parameter] public EventCallback<ScheduledDutyModel> Submitted { get; set; }
        [Parameter] public EventCallback Cancelled { get; set; }
        [Parameter] public long? scheduledDutyId { get; set; }
        private ScheduledDutyModel scheduledDuty { get; set; }
        private ValidatedForm _validatedForm;

        [Parameter] public bool Modal { get; set; }
        protected override async Task OnInitializedAsync() => await FreshenData();

        public override async Task FreshenData()
        {
            scheduledDuty=new ScheduledDutyModel();

            if (ScheduledDuty is not null)
                scheduledDuty = ScheduledDuty;

            if (ScheduledDuty is null && scheduledDutyId.HasValue)
                scheduledDuty = await app.dbContext.ScheduledDuties.FindAsync(scheduledDutyId);

            editContext = new EditContext(scheduledDuty);
        }
        public async Task OnSubmit()
        {
            try
            {
                if (scheduledDuty.CompletedOn.HasValue && !scheduledDuty.CompletedBy.HasValue)
                    scheduledDuty.CompletedBy = _auth.UserId();
                    

                long id=(scheduledDuty.Id <= 0) ? 
                    await app.api.ProcessCommand<ScheduledDutyModel, CreateScheduledDuty>("api/CreateScheduledDuty", new CreateScheduledDuty { ScheduledDuty = scheduledDuty }) : 
                    await app.api.ProcessCommand<ScheduledDutyModel, UpdateScheduledDuty>("api/UpdateScheduledDuty", new UpdateScheduledDuty { Duty=scheduledDuty });    
               
                if (id <= 0)
                    throw new Exception("Failed to save Scheduled Duty");

                scheduledDuty.Id = id;

                editContext = new EditContext(scheduledDuty);
                await Submitted.InvokeAsync(scheduledDuty);
                _validatedForm.HideModal();
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
        private async Task BreedingRecordSubmitted(BreedingRecordModel e)
        {
            if(!string.IsNullOrEmpty(e.Resolution) && e.ResolutionDate.HasValue)
                scheduledDuty.CompletedOn = e.ResolutionDate.Value;

            await OnSubmit();
        }

        private async Task SnoozeSubmitted(DateTime e)
        {
            scheduledDuty.DueOn = e.Date;
            scheduledDuty.CompletedOn = null;
            scheduledDuty.CompletedBy = null;
            await OnSubmit();
        }
        private bool showSnooze = false;
        private void Snooze()
        {
            showSnooze = true;
            StateHasChanged();
        }

        private async Task Cancel()
        {
            editContext = new EditContext(scheduledDuty);
            await Cancelled.InvokeAsync(scheduledDuty);
            _validatedForm.HideModal();
            StateHasChanged();
        }
    }
}