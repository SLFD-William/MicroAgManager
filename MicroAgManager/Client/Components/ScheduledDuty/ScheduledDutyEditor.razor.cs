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
        [Parameter] public long? scheduledDutyId { get; set; }
        private ScheduledDutyModel scheduledDuty { get; set; }
        private ValidatedForm _validatedForm;
        
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
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
        private async Task BreedingRecordSubmitted(object e)
        {
            var model = e as BreedingRecordModel;
            if(!string.IsNullOrEmpty(model?.Resolution) && model.ResolutionDate.HasValue)
                scheduledDuty.CompletedOn = model.ResolutionDate.Value;

            await OnSubmit();
        }

        private async Task SnoozeSubmitted(DateTime e)
        {
            showSnooze = false;
            scheduledDuty.DueOn = e.Date;
            scheduledDuty.CompletedOn = null;
            scheduledDuty.CompletedBy = null;
            await OnSubmit();
        }
        private bool showSnooze = false;
        private bool showConfirm = false;
        private void Snooze()=>showSnooze = true;
        private void SnoozeCancel()=>showSnooze = false;
            
        private void Confirm()=>showConfirm = true;
        private void ConfirmCancel() => showConfirm = false;
        private async Task DismissedConfirmed()
        {
            showConfirm = false;
            scheduledDuty.Dismissed = true;
            scheduledDuty.CompletedOn = DateTime.Now;
            await OnSubmit();
        }
        private async Task Cancel()
        {
            editContext = new EditContext(scheduledDuty);
            await Cancelled.InvokeAsync(scheduledDuty);
            StateHasChanged();
        }
    }
}