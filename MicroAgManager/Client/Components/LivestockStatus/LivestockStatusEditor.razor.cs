using BackEnd.BusinessLogic.Livestock.Status;
using Domain.Constants;
using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FrontEnd.Components.LivestockStatus
{
    public partial class LivestockStatusEditor : DataComponent<LivestockStatusModel>
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
        protected new LivestockStatusModel working { get => base.working as LivestockStatusModel; set { base.working = value; } }
        public override async Task FreshenData()
        {
            if (LivestockAnimal is not null)
                livestockAnimalId = LivestockAnimal.Id;

            working = new LivestockStatusModel()
            {
                LivestockAnimalId = livestockAnimalId.HasValue ? livestockAnimalId.Value:0,
                BeingManaged = LivestockStatusModeConstants.Unchanged,
                BottleFed = LivestockStatusModeConstants.Unchanged,
                ForSale = LivestockStatusModeConstants.Unchanged,
                InMilk = LivestockStatusModeConstants.Unchanged,
                Sterile = LivestockStatusModeConstants.Unchanged
            };

            if (LivestockStatus is not null)
                working = LivestockStatus;


            if (LivestockStatus is null && livestockStatusId > 0)
                working = await app.dbContext.LivestockStatuses.FindAsync(livestockStatusId);

            editContext = new EditContext(working);
        }
        public async Task OnSubmit()
        {
            try
            {
                long id=(working.Id <= 0)?
                    await app.api.ProcessCommand<LivestockStatusModel, CreateLivestockStatus>("api/CreateLivestockStatus", new CreateLivestockStatus { LivestockStatus = working }): 
                    await app.api.ProcessCommand<LivestockStatusModel, UpdateLivestockStatus>("api/UpdateLivestockStatus", new UpdateLivestockStatus { LivestockStatus = working });

                if (id <= 0)
                    throw new Exception("Failed to save livestock Status");

                working.Id = id;
                original = working.Clone() as LivestockStatusModel;
                editContext = new EditContext(working);
                await Submitted.InvokeAsync(working);
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
        private async Task Cancel()
        {
            editContext = new EditContext(working);
            await Cancelled.InvokeAsync(working);
            StateHasChanged();
        }
    }
}
