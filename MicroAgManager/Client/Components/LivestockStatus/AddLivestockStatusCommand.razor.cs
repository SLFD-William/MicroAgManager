using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.Livestock.Status;

namespace FrontEnd.Components.LivestockStatus
{
    public partial class AddLivestockStatusCommand : DataComponent
    {
        [CascadingParameter] public LivestockAnimalSummary LivestockAnimal { get; set; }
        [CascadingParameter] public LivestockStatusModel LivestockStatus { get; set; }
        [Parameter] public bool showUpdateCancelButtons { get; set; }
        [Parameter] public EditContext editContext { get; set; }
        [Parameter] public EventCallback<long> Submitted { get; set; }
        [Parameter] public EventCallback Cancelled { get; set; }
        [Parameter] public long? livestockAnimalId { get; set; }
        [Parameter] public string? Command { get; set; }

        private CommandButton _commandButton;
        protected override async Task OnInitializedAsync()
        {
            if (LivestockStatus is not null)
            {
                livestockStatus = LivestockStatus;
                editContext = new EditContext(livestockStatus);
                StateHasChanged();
                return;
            }
            if (LivestockAnimal is not null)
                livestockAnimalId = LivestockAnimal.Id;

            livestockStatus = new LivestockStatusModel() { LivestockAnimalId = livestockAnimalId.Value };
            editContext = new EditContext(livestockStatus);
        }

        private LivestockStatusModel livestockStatus;
        public override async Task FreshenData()
        {
            editContext = new EditContext(livestockStatus);
        }
        public async Task OnSubmit()
        {
            try
            {
                
                livestockStatus.Id = await app.api.ProcessCommand<LivestockStatusModel, CreateLivestockStatus>("api/CreateLivestockStatus", new CreateLivestockStatus { LivestockStatus = livestockStatus });
                _commandButton.HideModal();
                await Submitted.InvokeAsync(livestockStatus.Id);
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
        private async Task Cancel()
        {
            editContext = new EditContext(livestockStatus);
            _commandButton.HideModal();
            await Cancelled.InvokeAsync(livestockStatus);
            StateHasChanged();
        }
    }
}
