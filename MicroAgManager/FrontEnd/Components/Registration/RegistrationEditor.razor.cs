using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.Registration;
using Domain.Abstracts;
using FrontEnd.Components.Registrar;

namespace FrontEnd.Components.Registration
{
    public partial class RegistrationEditor : HasRecipientComponent<RegistrationModel>
    {
        [CascadingParameter] public RegistrationModel Registration { get; set; }
        [Parameter] public long? registrationId { get; set; }
        [Parameter] public required long registrarId { get; set; }

        private ValidatedForm _validatedForm;

        protected new RegistrationModel working { get => base.working as RegistrationModel; set { base.working = value; } }
        #region Registrar
        private bool showRegistrarModal = false;
        private RegistrarEditor _registrarEditor;
        private void ShowRegistrarEditor()
        {
            showRegistrarModal = true;
            StateHasChanged();
        }
        private void RegistrarCanceled()
        {
            working.RegistrarId = ((RegistrationModel)original).RegistrarId;
            showRegistrarModal = false;
            StateHasChanged();
        }
        private void RegistrarCreated(object e)
        {
            var status = e as BaseModel;
            showRegistrarModal = false;
            working.RegistrarId = status?.Id ?? 0;
            StateHasChanged();
        }
        #endregion
        public override async Task FreshenData()
        {
            if (Registration is not null)
                working = Registration;

            if (Registration is null && registrationId > 0)
                working = await app.dbContext.Registrations.FindAsync(registrationId);

            if (working is null && registrarId>0)
                working = new RegistrationModel()
                {
                    RecipientId = RecipientId,
                    RecipientTypeId = RecipientTypeId,
                    RecipientType = RecipientType,
                    RegistrarId = registrarId,
                    RegistrationDate = DateTime.Now,
                };
    
            SetEditContext(working);
        }
        public async Task OnSubmit()
        {
            try
            {

                long id = (working.Id <= 0) ?
                     await app.api.ProcessCommand<RegistrationModel, CreateRegistration>("api/CreateRegistration", new CreateRegistration { Registration = working }) :
                     await app.api.ProcessCommand<RegistrationModel, UpdateRegistration>("api/UpdateRegistration", new UpdateRegistration { Registration = working });

                if (id <= 0)
                    throw new Exception("Failed to save livestock Status");

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
            working = original.Map(working) as RegistrationModel;
            SetEditContext(working);
            await Cancelled.InvokeAsync(working);

        }
    }
}
