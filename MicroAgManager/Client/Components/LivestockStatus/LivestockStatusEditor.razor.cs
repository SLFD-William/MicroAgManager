using BackEnd.BusinessLogic.Livestock.Status;
using Domain.Constants;
using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockStatus
{
    public partial class LivestockStatusEditor:Editor<LivestockStatusModel>
    {
        private readonly static List<string> StatusModes = new List<string> {
            string.Empty,
            LivestockStatusModeConstants.Unchanged,
            LivestockStatusModeConstants.False,
            LivestockStatusModeConstants.True
        };
        [CascadingParameter] public LivestockAnimalModel LivestockAnimal { get; set; }
        
        [Parameter] public long? LivestockAnimalId { get; set; }
        [Parameter] public long? livestockStatusId { get; set; }
        private LivestockStatusModel livestockStatus;
        public override async Task FreshenData()
        {
            if (livestockStatus is not null)
            {
                editContext = new EditContext(livestockStatus);
                StateHasChanged();
                return;
            }
            if(LivestockAnimal is not null)
                LivestockAnimalId=LivestockAnimal.Id;

            livestockStatus= new LivestockStatusModel() { LivestockAnimalId = LivestockAnimalId.Value };
            var query = app.dbContext.LivestockStatuses.AsQueryable();
            if (livestockStatusId.HasValue && livestockStatusId > 0)
                query = query.Where(f => f.Id == livestockStatusId);
            if (LivestockAnimalId.HasValue && LivestockAnimalId > 0)
                query = query.Where(f => f.LivestockAnimalId == LivestockAnimalId);
            
            if(!createOnly)
                livestockStatus = await query.OrderBy(f => f.Id).FirstOrDefaultAsync() ?? new LivestockStatusModel() { LivestockAnimalId=LivestockAnimalId.Value };
            
            editContext = new EditContext(livestockStatus);
        }
        public override async Task OnSubmit()
        {
            try
            {
                _submitting = true;
                var state = livestockStatus.Id <= 0 ? EntityState.Added : EntityState.Modified;

                if (livestockStatus.Id <= 0)
                    livestockStatus.Id = await app.api.ProcessCommand<LivestockStatusModel, CreateLivestockStatus>("api/CreateLivestockStatus", new CreateLivestockStatus { LivestockStatus = livestockStatus });
                else
                    livestockStatus.Id = await app.api.ProcessCommand<LivestockStatusModel, UpdateLivestockStatus>("api/UpdateLivestockStatus", new UpdateLivestockStatus { LivestockStatus = livestockStatus });

                if (livestockStatus.Id <= 0)
                    throw new Exception("Failed to save livestock Status");

                editContext = new EditContext(livestockStatus);
                await Submitted.InvokeAsync(livestockStatus);
                _submitting = false;
                if (createOnly)
                {
                    livestockStatus = new LivestockStatusModel() { LivestockAnimalId = LivestockAnimalId.Value };
                    editContext = new EditContext(livestockStatus);
                    editContext.MarkAsUnmodified();
                    await Submitted.InvokeAsync(livestockStatus);
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
            finally { _submitting = false; }
        }
    }
}
