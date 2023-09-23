using BackEnd.BusinessLogic.Livestock.Status;
using Domain.Constants;
using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

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
        [Parameter] public bool showUpdateCancelButtons { get; set; }
        [Parameter] public EditContext editContext { get; set; }
        [Parameter] public EventCallback<LivestockStatusModel> Submitted { get; set; }
        [Parameter] public EventCallback Cancelled { get; set; }
        [Parameter] public long? livestockAnimalId { get; set; }
        [Parameter] public long? livestockStatusId { get; set; }

        protected override async Task OnInitializedAsync() => await FreshenData();

        private LivestockStatusModel livestockStatus;
        public override async Task FreshenData()
        {
            if (LivestockStatus is not null)
            {
                livestockStatus = LivestockStatus;
                editContext = new EditContext(livestockStatus);
                StateHasChanged();
                return;
            }
            if(LivestockAnimal is not null)
                livestockAnimalId=LivestockAnimal.Id;

            var query = app.dbContext.LivestockStatuses.AsQueryable();
            if (livestockStatusId.HasValue && livestockStatusId > 0)
                query = query.Where(f => f.Id == livestockStatusId);
            if (livestockAnimalId.HasValue && livestockAnimalId > 0)
                query = query.Where(f => f.LivestockAnimalId == livestockAnimalId);
            
            livestockStatus=await query.FirstOrDefaultAsync() ?? new LivestockStatusModel() { LivestockAnimalId = livestockAnimalId.Value };

            editContext = new EditContext(livestockStatus);
        }
        public async Task OnSubmit()
        {
            try
            {
                if (livestockStatus.Id <= 0)
                    livestockStatus.Id = await app.api.ProcessCommand<LivestockStatusModel, CreateLivestockStatus>("api/CreateLivestockStatus", new CreateLivestockStatus { LivestockStatus = livestockStatus });
                else
                    livestockStatus.Id = await app.api.ProcessCommand<LivestockStatusModel, UpdateLivestockStatus>("api/UpdateLivestockStatus", new UpdateLivestockStatus { LivestockStatus = livestockStatus });

                if (livestockStatus.Id <= 0)
                    throw new Exception("Failed to save livestock Status");

                editContext = new EditContext(livestockStatus);
                await Submitted.InvokeAsync(livestockStatus);
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
            StateHasChanged();
        }
    }
}
