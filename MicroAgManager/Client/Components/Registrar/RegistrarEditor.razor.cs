using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.Registrar;

namespace FrontEnd.Components.Registrar
{
    public partial class RegistrarEditor : DataComponent
    {
        [CascadingParameter] public RegistrarModel Registrar { get; set; }
        [Parameter] public long? registrarId { get; set; }
        private RegistrarModel registrar { get; set; }
        private ValidatedForm _validatedForm;
        public void HideModal() => _validatedForm.HideModal();
        public void ShowModal() => _validatedForm.ShowModal();
        protected override async Task OnInitializedAsync() => await FreshenData();
        public override async Task FreshenData()
        {
            registrar = new RegistrarModel();

            if (Registrar is not null)
                registrar    = Registrar;

            if (Registrar is null && registrarId.HasValue)
                registrar = await app.dbContext.Registrars.FindAsync(registrarId);

            editContext = new EditContext(registrar);
        }
        public async Task OnSubmit()
        {
            try
            {
                long id = (registrar.Id <= 0) ?
                    await app.api.ProcessCommand<RegistrarModel, CreateRegistrar>("api/CreateRegistrar", new CreateRegistrar { Registrar = registrar }) :
                    await app.api.ProcessCommand<RegistrarModel, UpdateRegistrar>("api/UpdateRegistrar", new UpdateRegistrar { Registrar = registrar });

                if (id <= 0)
                    throw new Exception("Failed to save Registrar");
                registrar.Id = id;

                editContext = new EditContext(registrar);
                await Submitted.InvokeAsync(registrar);
                _validatedForm.HideModal();
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
        private async Task Cancel()
        {
            editContext = new EditContext(registrar);
            await Cancelled.InvokeAsync(registrar);
            _validatedForm.HideModal();
            StateHasChanged();
        }
    }
}
