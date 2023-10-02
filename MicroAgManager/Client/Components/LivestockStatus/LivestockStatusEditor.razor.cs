using BackEnd.BusinessLogic.Livestock.Status;
using Domain.Constants;
using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FrontEnd.Components.LivestockStatus
{
    public partial class LivestockStatusEditor : DataComponent
    {
        private readonly static List<string> StatusModes = new List<string> {
            string.Empty,
            LivestockStatusModeConstants.Unchanged,
            LivestockStatusModeConstants.False,
            LivestockStatusModeConstants.True
        };
        [CascadingParameter] public LivestockAnimalSummary LivestockAnimal { get; set; }
        [CascadingParameter] public LivestockStatusModel LivestockStatus { get; set; }
        [Parameter] public long? livestockAnimalId { get; set; }
        [Parameter] public long? livestockStatusId { get; set; }
        private ValidatedForm _validatedForm;
        protected override async Task OnInitializedAsync() => await FreshenData();
        public void HideModal() => _validatedForm.HideModal();
        public void ShowModal() => _validatedForm.ShowModal();
        private LivestockStatusModel livestockStatus;
        public override async Task FreshenData()
        {
            if (LivestockAnimal is not null)
                livestockAnimalId = LivestockAnimal.Id;

            livestockStatus = new LivestockStatusModel()
            {
                LivestockAnimalId = livestockAnimalId.HasValue ? livestockAnimalId.Value:0,
                BeingManaged = LivestockStatusModeConstants.Unchanged,
                BottleFed = LivestockStatusModeConstants.Unchanged,
                ForSale = LivestockStatusModeConstants.Unchanged,
                InMilk = LivestockStatusModeConstants.Unchanged,
                Sterile = LivestockStatusModeConstants.Unchanged
            };

            if (LivestockStatus is not null)
                livestockStatus = LivestockStatus;


            if (LivestockStatus is null && livestockStatusId > 0)
                livestockStatus = await app.dbContext.LivestockStatuses.FindAsync(livestockStatusId);

            editContext = new EditContext(livestockStatus);
        }
        public async Task OnSubmit()
        {
            try
            {
                long id=(livestockStatus.Id <= 0)?
                    await app.api.ProcessCommand<LivestockStatusModel, CreateLivestockStatus>("api/CreateLivestockStatus", new CreateLivestockStatus { LivestockStatus = livestockStatus }): 
                    await app.api.ProcessCommand<LivestockStatusModel, UpdateLivestockStatus>("api/UpdateLivestockStatus", new UpdateLivestockStatus { LivestockStatus = livestockStatus });

                if (id <= 0)
                    throw new Exception("Failed to save livestock Status");

                livestockStatus.Id = id;
                editContext = new EditContext(livestockStatus);
                await Submitted.InvokeAsync(livestockStatus);
                _validatedForm.HideModal();
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
        private async Task Cancel()
        {
            editContext = new EditContext(livestockStatus);
            await Cancelled.InvokeAsync(livestockStatus);
            _validatedForm.HideModal();
            StateHasChanged();
        }
    }
}
