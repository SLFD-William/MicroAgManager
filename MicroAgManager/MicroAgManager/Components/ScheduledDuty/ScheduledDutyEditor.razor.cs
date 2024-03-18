using BackEnd.BusinessLogic.Livestock;
using BackEnd.BusinessLogic.ScheduledDuty;
using Domain.Abstracts;
using Domain.Constants;
using Domain.Interfaces;
using Domain.Logic;
using Domain.Models;
using MicroAgManager.Components.Event;
using MicroAgManager.Components.Livestock;
using MicroAgManager.Components.Measurement;
using MicroAgManager.Components.Registration;
using MicroAgManager.Components.Shared;
using MicroAgManager.Components.TreatmentRecord;
using MicroAgManager.Data;
using MicroAgManager.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace MicroAgManager.Components.ScheduledDuty
{
    public partial class ScheduledDutyEditor: BaseEditor
    {
        [CascadingParameter] private ApplicationState appState { get; set; }
        [Inject] protected IAPIService api { get; set; }
        [Parameter] public Dictionary<string, long>? RecipientTypes { get; set; }

        private DynamicComponent? recordComponent;
        private ScheduledDutyModel? scheduledDuty;
        private bool reschedule = false;
        private DateTime? newDue = null;
        private long DutyId
        {
            get
            {
                return scheduledDuty.DutyId;
            }
            set
            {
                if (value != scheduledDuty.DutyId)
                {
                    scheduledDuty.DutyId = value;
                    scheduledDuty.Duty = appState.DbContext.Duties.Find(value);
                    ShowRecordEditor();
                    StateHasChanged();
                }
            }
        }
        private long ScheduleSourceId
        {
            get
            {
                return scheduledDuty.ScheduleSourceId;
            }
            set
            {
                if (value != scheduledDuty.ScheduleSourceId)
                {
                    scheduledDuty.ScheduleSourceId = value;
                    ShowRecordEditor();
                    StateHasChanged();
                }
            }
        }
        protected override void OnInitialized()
        {
            scheduledDuty = editContext.Model as ScheduledDutyModel;
            if (scheduledDuty is null) return;
            if (string.IsNullOrEmpty(scheduledDuty.ScheduleSource)) scheduledDuty.ScheduleSource = ScheduledDutySourceConstants.Event;
            if (scheduledDuty.DueOn == DateTime.MinValue) scheduledDuty.DueOn = DateTime.Today;
            if (scheduledDuty.DutyId > 0)
                scheduledDuty.Duty = appState.DbContext.Duties.Find(scheduledDuty.DutyId);
            DutyId = scheduledDuty.DutyId;
            ScheduleSourceId = scheduledDuty.ScheduleSourceId;
        }

        private DateTime? GetDatePerformedFromRecord(object recordModel)
        {
            switch (recordModel.GetType().Name)
            {
                case nameof(MeasurementModel):
                    return ((MeasurementModel)recordModel).DatePerformed;
                case nameof(RegistrationModel):
                    return ((RegistrationModel)recordModel).RegistrationDate;
                case nameof(ServiceLivestock):
                    return ((ServiceLivestock)recordModel).ServiceDate;
                case nameof(BreedingRecordModel):
                    return ((BreedingRecordModel)recordModel).ResolutionDate;
                case nameof(TreatmentRecordModel):
                    return ((TreatmentRecordModel)recordModel).DatePerformed;
                default:
                    return null;
            }
        }
        private string GetRecipientFromRecipientType(DutyModel duty)
        {
            switch (duty.RecipientType)
            {
                case nameof(RecipientTypeConstants.LivestockAnimal):
                case nameof(RecipientTypeConstants.LivestockBreed):
                    return nameof(Domain.Entity.Livestock);

                default:
                    return null;
            }
        }

        private async Task OnScheduledDutySubmit(EditContext editContext)
        {
            var myComponent = recordComponent?.Instance as HasRecipientsEditor;
            if (!myComponent.editContext.Validate()) return;

            var recordModel = await myComponent.SubmitEditContext();
            if (recordModel is ServiceLivestock)
            {
                scheduledDuty.RecordId = -1;
                scheduledDuty.Record = nameof(ServiceLivestock);
                scheduledDuty.RecipientId = ((ServiceLivestock)recordModel).StudId;
            }
            else if (recordModel is IHasRecipient)
            {
                scheduledDuty.Record = ((BaseModel)recordModel).GetEntityName();
                scheduledDuty.RecordId = ((BaseModel)recordModel).Id;
                scheduledDuty.RecipientId = ((IHasRecipient)recordModel).RecipientId;
            }


            scheduledDuty.Recipient = GetRecipientFromRecipientType(scheduledDuty.Duty);
            scheduledDuty.CompletedOn = GetDatePerformedFromRecord(recordModel);
            scheduledDuty.CompletedBy = appState.UserId;
            if (await RescheduleIsNeeded()) return;
            await SubmitScheduledDutyChange();
        }
        private async Task SubmitScheduledDutyChange()
        {
            editContext = new EditContext(scheduledDuty);

            if (!editContext.Validate()) return;
            try
            {

                var creating = (scheduledDuty.Id <= 0);

                var id = creating ?
                    await api.ProcessCommand<ScheduledDutyModel, BackEnd.BusinessLogic.ScheduledDuty.CreateScheduledDuty>("api/CreateScheduledDuty", new BackEnd.BusinessLogic.ScheduledDuty.CreateScheduledDuty { ScheduledDuty = scheduledDuty, Reschedule = reschedule, RescheduleDueOn = newDue }) :
                    await api.ProcessCommand<ScheduledDutyModel, UpdateScheduledDuty>("api/UpdateScheduledDuty", new UpdateScheduledDuty { ScheduledDuty = scheduledDuty, Reschedule = reschedule, RescheduleDueOn = newDue });

                if (id <= 0)
                    throw new Exception("Unable to save scheduled duty");
                if (creating)
                {
                    scheduledDuty.Id = id;
                    appState.DbContext.ScheduledDuties.Add(scheduledDuty);
                }
                else
                {
                    var updated = await appState.DbContext.ScheduledDuties.FindAsync(scheduledDuty.Id);
                    updated = scheduledDuty.Map(updated) as ScheduledDutyModel;
                }
                await appState.DbContext.SaveChangesAsync();
                editContext = new EditContext(scheduledDuty);
                await OnSubmit.InvokeAsync(editContext);
            }
            catch (Exception ex)
            {

            }
        }
        #region Reschedule
        private bool showReschedule = false;
        private TimePrompt? _rescheduleEditor;
        private async Task RescheduleCancel()
        {
            showReschedule = false;
            newDue = null;
            reschedule = false;
            await SubmitScheduledDutyChange();
        }

        private async Task RescheduleSubmitted(DateTime e)
        {
            showReschedule = false;
            newDue = e.Date;
            reschedule = true;
            await SubmitScheduledDutyChange();
        }
        private async Task<bool> RescheduleIsNeeded()
        {
            showReschedule = (scheduledDuty?.Duty is IHasFrequencyAndDuration || scheduledDuty?.ScheduleSource == ScheduledDutySourceConstants.Chore);
            if (!showReschedule) return showReschedule;
            _rescheduleEditor.SuggestedTime =await GetNextScheduledDutyDueDate();
            return _rescheduleEditor.SuggestedTime.HasValue;
        }
        private async Task< DateTime?> GetNextScheduledDutyDueDate()
        {
            if (scheduledDuty?.ScheduleSource == ScheduledDutySourceConstants.Chore)
                return await DutyLogic.GetNextChoreDueDate(appState.DbContext, scheduledDuty);
            return await DutyLogic.GetNextFreqAndDurationDueDate(appState.DbContext, scheduledDuty);
        }


        #endregion
        #region Snooze
        private bool showSnooze = false;
        private TimePrompt? _snoozeEditor;
        private void Snooze() => showSnooze = true;
        private void SnoozeCancel() => showSnooze = false;

        private async Task SetSuggestedSnooze()
        {
            var snoozeTime = DateTime.Today;
            if (_recordType == typeof(BreedingRecordEditor))
            {
                var breedingRecord = await appState.DbContext.BreedingRecords.FindAsync(scheduledDuty?.RecordId);
                if (breedingRecord is not null)
                {
                    var animal = await appState.DbContext.Livestocks
                        .Include(p => p.Breed)
                        .FirstOrDefaultAsync(i => i.Id == breedingRecord.FemaleId);
                    double.TryParse(animal?.Breed?.GestationPeriod.ToString(), out var days);
                    snoozeTime = breedingRecord.ServiceDate.AddDays(days);

                }
            }
            if (snoozeTime <= DateTime.Today) return;
            _snoozeEditor.SuggestedTime = snoozeTime;
            SuggestedSnooze = snoozeTime;
            StateHasChanged();
        }

        private string SnoozeLabel() => SuggestedSnooze.HasValue ? $"Snooze until {SuggestedSnooze.Value.ToShortDateString()}" : "Snooze";
        public DateTime? SuggestedSnooze { get; private set; }
        private async Task SnoozeSubmitted(DateTime e)
        {
            showSnooze = false;
            scheduledDuty.DueOn = e.Date;
            scheduledDuty.CompletedOn = null;
            scheduledDuty.CompletedBy = null;
            await SubmitScheduledDutyChange();
        }

        #endregion
        #region Dismiss
        private bool showDismiss = false;
        private void Dismiss() => showDismiss = true;
        private void DismissCancel() => showDismiss = false;
        private async Task DismissedConfirmed()
        {
            showDismiss = false;
            scheduledDuty.Dismissed = true;
            scheduledDuty.CompletedOn = DateTime.Now;
            scheduledDuty.CompletedBy = appState.UserId;
            await SubmitScheduledDutyChange();
        }
        #endregion
        #region Duty
        private List<DutyModel> dutySelections()
        {
            var query = appState.DbContext.Duties.Where(d => d.Command == scheduledDuty.Duty.Command);
            if (RecipientTypes?.Any() == true)
                query = query.Where(d => RecipientTypes.Any(rt => rt.Key == d.RecipientType && rt.Value == d.RecipientTypeId));

            var result = query.ToList();

            if (result.Count() == 1)
                DutyId = result.First().Id;

            return result;
        }
        private bool _showingDutyEditor = false;
        private EditContext dutyEditContext = new EditContext(new DutyModel());
        private void ShowDutyEditor()
        {
            _showingDutyEditor = true;
            StateHasChanged();
        }
        private void DutySubmitted(EditContext e)
        {
            var model = e.Model as DutyModel;
            _showingDutyEditor = false;
            if (model == null) return;
            scheduledDuty.DutyId = model.Id;
            scheduledDuty.Duty = appState.DbContext.Duties.Find(model.Id);
            editContext = new EditContext(scheduledDuty);
            StateHasChanged();
        }
        private void DutyCanceled(object e)
        {
            _showingDutyEditor = false;
            StateHasChanged();
        }

        #endregion
        #region Source
        
        private bool _showingSourceEditor = false;
        Type? sourceType;
        private EditContext sourceEditContext;
        private Dictionary<string, ComponentMetadata> sourceComponents =
         new()
             {
            {
                nameof(EventEditor),
                new ComponentMetadata
                {
                    Name = "Event",
                    Parameters = new() {
                        { "Modal", true },
                        { "Show", true },
                        { "editContext", string.Empty },
                        { "OnCancel", string.Empty },
                        { "OnSubmit", string.Empty }
                    }
                }
            }
             };
        private void ShowSourceEditor()
        {
            object model = null;
            switch (scheduledDuty.ScheduleSource)
            {
                case nameof(ScheduledDutySourceConstants.Event):
                    model = appState.DbContext.Events.Find(scheduledDuty.ScheduleSourceId) ?? new EventModel() { Color = "#ffffff", StartDate = DateTime.Today, EndDate = DateTime.Today };
                    sourceType = typeof(EventEditor);
                    break;
                case nameof(ScheduledDutySourceConstants.Chore):
                    break;
                case nameof(ScheduledDutySourceConstants.Milestone):
                    break;
                default:
                    sourceType = null;
                    break;
            }
            if (sourceType is not null)
            {
                sourceEditContext = new EditContext(model);
                sourceComponents[sourceType.Name].Parameters["editContext"] = sourceEditContext;
                sourceComponents[sourceType.Name].Parameters["OnCancel"] = EventCallback.Factory.Create<EditContext>(this, SourceCanceled);
                sourceComponents[sourceType.Name].Parameters["OnSubmit"] = EventCallback.Factory.Create<EditContext>(this, SourceSubmitted);
            }
            _showingSourceEditor = true;
            StateHasChanged();
        }
        private void SourceSubmitted(EditContext e)
        {
            scheduledDuty.ScheduleSourceId = ((BaseModel)e.Model).Id;
            _showingSourceEditor = false;
            StateHasChanged();
        }
        private void SourceCanceled(EditContext e)
        {
            _showingSourceEditor = false;
            StateHasChanged();
        }

        #endregion
        #region Record
        private bool _showingRecordEditor = false;
        Type? _recordType;
        private Dictionary<string, ComponentMetadata> recordComponents =
         new()
            {
            {
                nameof(TreatmentRecordEditor),
                new ComponentMetadata
                {
                    Name = "TreatmentRecord",
                    Parameters = new() {
                        { "Modal", false },
                        { "Show", true },
                        { "ShowUpdateCancel", false },
                        { "editContext", string.Empty }
                    }
                }
            },{
                nameof(MeasurementEditor),
                new ComponentMetadata
                {
                    Name = "Measurement",
                    Parameters = new() {
                        { "Modal", false },
                        { "Show", true },
                        { "ShowUpdateCancel", false },
                        { "editContext", string.Empty }
                    }
                }
            },
            {
                nameof(RegistrationEditor),
                new ComponentMetadata
                {
                    Name = "Registration",
                    Parameters = new() {
                        { "Modal", false },
                        { "Show", true },
                        { "ShowUpdateCancel", false },
                        { "editContext", string.Empty }
                    }
                }
            },
            {
                nameof(ServiceLivestockEditor),
                new ComponentMetadata
                {
                    Name = "Service",
                    Parameters = new() {
                        { "Modal", false },
                        { "Show", true },
                        { "ShowUpdateCancel", false },
                        { "editContext", string.Empty }
                    }
                }
            },
            {
                nameof(BreedingRecordEditor),
                new ComponentMetadata
                {
                    Name = "BreedingRecord",
                    Parameters = new() {
                        { "Modal", false },
                        { "Show", true },
                        { "ShowUpdateCancel", false },
                        { "editContext", string.Empty }
                    }
                }
            }
             };
        private bool ShowRecordEditor()
        {
            if (scheduledDuty.ScheduleSourceId < 1 || scheduledDuty.DutyId < 1) return false;
            IHasRecipient model = null;
            switch (scheduledDuty.Duty.Command)
            {
                case nameof(DutyCommandConstants.Registration):
                    scheduledDuty.Record = nameof(DutyCommandConstants.Registration);
                    model = appState.DbContext.Registrations.Find(scheduledDuty.RecordId) ?? new RegistrationModel() { RecipientId = scheduledDuty.RecipientId };
                    if (((RegistrationModel)model).Id < 1)
                    {
                        ((RegistrationModel)model).RegistrarId = scheduledDuty.Duty.CommandId;
                        ((RegistrationModel)model).Registrar = appState.DbContext.Registrars.Find(scheduledDuty.Duty.CommandId);
                        ((RegistrationModel)model).RegistrationDate = DateTime.Today;
                    }
                    _recordType = typeof(RegistrationEditor);
                    break;
                case nameof(DutyCommandConstants.Measurement):
                    scheduledDuty.Record = nameof(DutyCommandConstants.Measurement);
                    model = appState.DbContext.Measurements.Find(scheduledDuty.RecordId) ?? new MeasurementModel() { RecipientId = scheduledDuty.RecipientId };
                    if (((MeasurementModel)model).Id < 1)
                    {
                        ((MeasurementModel)model).MeasureId = scheduledDuty.Duty.CommandId;
                        ((MeasurementModel)model).Measure = appState.DbContext.Measures.Find(scheduledDuty.Duty.CommandId);
                        ((MeasurementModel)model).DatePerformed = DateTime.Today;
                    }
                    _recordType = typeof(MeasurementEditor);
                    break;
                case nameof(DutyCommandConstants.Service):
                    _recordType = typeof(ServiceLivestockEditor);
                    model = new ServiceLivestock()
                    {
                        RecipientId = scheduledDuty.RecipientId,
                        DamIds = new List<long>(),
                        ServiceDate = DateTime.Now,
                        ScheduleSource = scheduledDuty.ScheduleSource,
                        ScheduleSourceId = scheduledDuty.ScheduleSourceId
                    };
                    break;
                case nameof(DutyCommandConstants.Birth):
                case nameof(DutyCommandConstants.Breed):
                    scheduledDuty.Record = "BreedingRecord";
                    model = appState.DbContext.BreedingRecords.Find(scheduledDuty.RecordId) ?? new BreedingRecordModel() { RecipientId = scheduledDuty.RecipientId };
                    _recordType = typeof(BreedingRecordEditor);
                    break;
                case nameof(DutyCommandConstants.Treatment):
                    scheduledDuty.Record = nameof(DutyCommandConstants.Treatment);
                    model = appState.DbContext.TreatmentRecords.Find(scheduledDuty.RecordId) ?? new TreatmentRecordModel() { RecipientId = scheduledDuty.RecipientId };
                    if (((TreatmentRecordModel)model).Id < 1)
                    {
                        ((TreatmentRecordModel)model).TreatmentId = scheduledDuty.Duty.CommandId;
                        ((TreatmentRecordModel)model).Treatment = appState.DbContext.Treatments.Find(scheduledDuty.Duty.CommandId);
                        ((TreatmentRecordModel)model).DatePerformed = DateTime.Today;
                    }
                    _recordType = typeof(TreatmentRecordEditor);
                    break;
                default:
                    _recordType = null;
                    break;
            }
            if (_recordType is not null)
            {
                model.RecipientType = scheduledDuty.Duty.RecipientType;
                model.RecipientTypeId = scheduledDuty.Duty.RecipientTypeId;
                recordComponents[_recordType.Name].Parameters["editContext"] = new EditContext(model);
                return true;
            }
            return false;

        }
        private void RecordSubmitted(EditContext e)
        {
            scheduledDuty.RecordId = ((BaseModel)e.Model).Id;
            //_showingRecordEditor = false;
            StateHasChanged();
        }
        private void RecordCanceled(EditContext e)
        {
            //_showingRecordEditor = false;
            StateHasChanged();
        }

        #endregion
    }
}
