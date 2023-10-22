using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.ScheduledDuty;
using FrontEnd.Services;
using FrontEnd.Components.BreedingRecord;
using FrontEnd.Components.Measurement;
using FrontEnd.Components.TreatmentRecord;
using FrontEnd.Components.Registration;

namespace FrontEnd.Components.ScheduledDuty
{
    public partial class ScheduledDutyEditor : DataComponent<ScheduledDutyModel>
    {
        [Inject] FrontEndAuthenticationStateProvider _auth { get; set; }
        [CascadingParameter] public ScheduledDutyModel ScheduledDuty { get; set; }
        [Parameter] public long? scheduledDutyId { get; set; }
        private DutyModel _duty { get; set; }

        private ValidatedForm _validatedForm;
        protected new ScheduledDutyModel working { get => base.working as ScheduledDutyModel; set { base.working = value; } }
        public override async Task FreshenData()
        {
            working=new ScheduledDutyModel();

            if (ScheduledDuty is not null)
                working = ScheduledDuty;

            if (ScheduledDuty is null && scheduledDutyId.HasValue)
                working = await app.dbContext.ScheduledDuties.FindAsync(scheduledDutyId);

            _duty = await app.dbContext.Duties.FindAsync(working.DutyId);
            SetEditContext(working);
            await SetSuggestedSnooze();
        }
        public async Task OnSubmit()
        {
            try
            {
                if (working.CompletedOn.HasValue && !working.CompletedBy.HasValue)
                    working.CompletedBy = _auth.UserId();
                    

                var id=(working.Id <= 0) ? 
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
        private async Task Submit()
        {
            if (working.Record == "BreedingRecord" && _breedingRecordEditor.editContext.Validate())
                await _breedingRecordEditor.OnSubmit();
            if (working.Record == "Measurement" && _measurementEditor.editContext.Validate())
                await _measurementEditor.OnSubmit();
            if (working.Record == "Treatment" && _treatmentRecordEditor.editContext.Validate())
                await _treatmentRecordEditor.OnSubmit();
            StateHasChanged();
        }
        private async Task Cancel()
        {
            working = original.Clone() as ScheduledDutyModel;
            SetEditContext(working);
            await Cancelled.InvokeAsync(working);
        }
        #region BreedingRecord
        private BreedingRecordEditor _breedingRecordEditor;
        private async Task BreedingRecordSubmitted(object e)
        {
            var model = e as BreedingRecordModel;
            if(!string.IsNullOrEmpty(model?.Resolution) && model.ResolutionDate.HasValue)
                working.CompletedOn = model.ResolutionDate.Value;

            await OnSubmit();
        }
        #endregion
        #region Measurement
        private MeasurementEditor _measurementEditor; 
        private async Task MeasurementSubmitted(object e)
        {
            var model = e as MeasurementModel;
            working.RecordId = model.Id;
            working.CompletedOn = model.DatePerformed;
            await OnSubmit();
        }
        #endregion
        #region TreatmentRecord
        private TreatmentRecordEditor _treatmentRecordEditor;
        private async Task TreatmentRecordSubmitted(object e)
        {
            var model = e as TreatmentRecordModel;
            working.RecordId = model.Id;
            working.CompletedOn = model.DatePerformed;
            await OnSubmit();
        }
        #endregion
        #region Registration
        private RegistrationEditor _registrationEditor;
        private async Task RegistrationSubmitted(object e)
        {
            var model = e as RegistrationModel;
            working.RecordId = model.Id;
            working.CompletedOn = model.RegistrationDate;
            await OnSubmit();
        }
        #endregion
        #region Snooze
        private bool showSnooze = false;
        private Snooze? _snoozeEditor;
        private void Snooze() => showSnooze = true;
        private void SnoozeCancel() => showSnooze = false;

        private async Task SetSuggestedSnooze()
        {
            if (working.Record == "BreedingRecord")
            {
                var breedingRecord = await app.dbContext.BreedingRecords.FindAsync(working.RecordId);
                if (breedingRecord is not null)
                {
                    var animal = await app.dbContext.Livestocks.FindAsync(breedingRecord.FemaleId);
                    var breed = await app.dbContext.LivestockBreeds.FindAsync(animal?.LivestockBreedId);
                    double.TryParse(breed?.GestationPeriod.ToString(), out var days);
                    SuggestedSnooze = breedingRecord.ServiceDate.AddDays(days);
                }
            }
        }

        private string SnoozeLabel() => SuggestedSnooze.HasValue ? $"Snooze until {SuggestedSnooze.Value.ToShortDateString()}" : "Snooze";
        public DateTime? SuggestedSnooze { get; private set; }
        private async Task SnoozeSubmitted(DateTime e)
        {
            showSnooze = false;
            working.DueOn = e.Date;
            working.CompletedOn = null;
            working.CompletedBy = null;
            await OnSubmit();
        }
        
        #endregion
        #region Confirm
        private bool showConfirm = false;
                    
        private void Confirm()=>showConfirm = true;
        private void ConfirmCancel() => showConfirm = false;
        private async Task DismissedConfirmed()
        {
            showConfirm = false;
            working.Dismissed = true;
            working.CompletedOn = DateTime.Now;
            await OnSubmit();
        }
        #endregion

    }
}