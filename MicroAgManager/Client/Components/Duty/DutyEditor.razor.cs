using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.Duty;
using Domain.Constants;
using FrontEnd.Components.Registrar;
using Domain.Abstracts;
using FrontEnd.Components.Measure;

namespace FrontEnd.Components.Duty
{
    public partial class DutyEditor : DataComponent<DutyModel>
    {
        [CascadingParameter] public DutyModel Duty { get; set; }
        [Parameter] public long? dutyId { get; set; }
        
        private ValidatedForm _validatedForm;
        private RegistrarEditor _registrarEditor;
        private MeasureEditor _measureEditor;
        protected new DutyModel working { get => base.working as DutyModel; set { base.working = value; } }
        private string Command { get=>working.Command;
            set { working.Command = value;
                if (working.Command == DutyCommandConstants.Complete)
                {
                    working.RecipientType = nameof(RecipientTypeConstants.None);
                    working.RecipientTypeId = 0;
                    working.Relationship = nameof(DutyRelationshipConstants.Self);
                }
            } }
        
        public override async Task FreshenData()
        {
            working = new DutyModel();

            if (Duty is not null)
                working = Duty;

            if (Duty is null && dutyId.HasValue)
                working = await app.dbContext.Duties.FindAsync(dutyId);

            SetEditContext(working);
        }
        public async Task OnSubmit()
        {
            try
            {
                long id = (working.Id <= 0) ?
                    await app.api.ProcessCommand<DutyModel, CreateDuty>("api/CreateDuty", new CreateDuty { Duty = working }) :
                    await app.api.ProcessCommand<DutyModel, UpdateDuty>("api/UpdateDuty", new UpdateDuty { Duty = working });

                if (id <= 0)
                    throw new Exception("Failed to save Duty");

                working.Id = id;
                SetEditContext(working);
                await Submitted.InvokeAsync(working);
            }
            catch (Exception ex)
            {
               
            }
        }
        private async Task Cancel()
        {
            working =original.Map(working) as DutyModel;
            SetEditContext(working);
            await Cancelled.InvokeAsync(working);
        }
        private List<KeyValuePair<long, string>> recipientTypeIds()
        {
            switch (working.RecipientType)
            {
                case nameof(RecipientTypeConstants.LivestockAnimal):
                    return app.dbContext.LivestockAnimals.OrderBy(a=>a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
                default:
                    return new List<KeyValuePair<long, string>>();
            }
        }
        private List<KeyValuePair<long, string>> commandIds()
        {
            switch (working.Command)
            {
                case nameof(DutyCommandConstants.Registration):
                    return app.dbContext.Registrars.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
                case nameof(DutyCommandConstants.Measurement):
                    return app.dbContext.Measures.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
                default:
                    return new List<KeyValuePair<long, string>>();
            }
        }
        private bool showCommandId()
        {
            switch (working.Command)
            {
                case "":
                case null:
                case nameof(DutyCommandConstants.Birth):
                case nameof(DutyCommandConstants.Breed):
                case nameof(DutyCommandConstants.Complete):
                case nameof(DutyCommandConstants.Death):
                case nameof(DutyCommandConstants.Service):
                    return false;
                default:
                    return true;
            }
        }
        private string commandLabel()
        {
            switch (working.Command)
            {
                case nameof(DutyCommandConstants.Registration):
                    return "Registrar";
                case nameof(DutyCommandConstants.Measurement):
                    return "Measure";
                default:
                    return string.Empty;
            }
        }


        private bool showCommandModal=false;

        private void ShowCommandEditor()
        {
            showCommandModal = true;
            StateHasChanged();
        }
        private void CommandCanceled()
        {
            working.CommandId = ((DutyModel)original).CommandId;
            showCommandModal = false;
            StateHasChanged();
        }
        private void CommandCreated(object e)
        {
            var status = e as BaseModel;
            showCommandModal = false;
            working.CommandId = status?.Id ?? 0;
            StateHasChanged();
        }
    }
}
