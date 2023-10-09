using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.ScheduledDuty;
using FrontEnd.Services;

namespace FrontEnd.Components.ScheduledDuty
{
    public partial class ScheduledDutyEditor : DataComponent<ScheduledDutyModel>
    {
        [Inject] FrontEndAuthenticationStateProvider _auth { get; set; }
        [CascadingParameter] public ScheduledDutyModel ScheduledDuty { get; set; }
        [Parameter] public long? scheduledDutyId { get; set; }
        private ValidatedForm _validatedForm;
        protected new ScheduledDutyModel working { get => base.working as ScheduledDutyModel; set { base.working = value; } }
        public override async Task FreshenData()
        {
            working=new ScheduledDutyModel();

            if (ScheduledDuty is not null)
                working = ScheduledDuty;

            if (ScheduledDuty is null && scheduledDutyId.HasValue)
                working = await app.dbContext.ScheduledDuties.FindAsync(scheduledDutyId);

            editContext = new EditContext(working);
        }
        public async Task OnSubmit()
        {
            try
            {
                if (working.CompletedOn.HasValue && !working.CompletedBy.HasValue)
                    working.CompletedBy = _auth.UserId();
                    

                long id=(working.Id <= 0) ? 
                    await app.api.ProcessCommand<ScheduledDutyModel, CreateScheduledDuty>("api/CreateScheduledDuty", new CreateScheduledDuty { ScheduledDuty = working }) : 
                    await app.api.ProcessCommand<ScheduledDutyModel, UpdateScheduledDuty>("api/UpdateScheduledDuty", new UpdateScheduledDuty { Duty=working });    
               
                if (id <= 0)
                    throw new Exception("Failed to save Scheduled Duty");

                working.Id = id;
                SetEditContext(working);
                await Submitted.InvokeAsync(working);
            }
            catch (Exception ex)
            {

            }
        }
        private async Task BreedingRecordSubmitted(object e)
        {
            var model = e as BreedingRecordModel;
            if(!string.IsNullOrEmpty(model?.Resolution) && model.ResolutionDate.HasValue)
                working.CompletedOn = model.ResolutionDate.Value;

            await OnSubmit();
        }

        private async Task SnoozeSubmitted(DateTime e)
        {
            showSnooze = false;
            working.DueOn = e.Date;
            working.CompletedOn = null;
            working.CompletedBy = null;
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
            working.Dismissed = true;
            working.CompletedOn = DateTime.Now;
            await OnSubmit();
        }
        private async Task Cancel()
        {   
            working = original.Clone() as ScheduledDutyModel;
            SetEditContext(working);
            await Cancelled.InvokeAsync(working);
        }
    }
}