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
        [CascadingParameter] public LivestockTypeModel livestockType { get; set; }
        
        [Parameter] public long? livestockTypeId { get; set; }
        [Parameter] public long? livestockStatusId { get; set; }
        private LivestockStatusModel livestockStatus;
        protected override async Task FreshenData()
        {
            if (dbContext is null) dbContext = await dbSync.GetPreparedDbContextAsync();
            if (livestockStatus is not null)
            {
                editContext = new EditContext(livestockStatus);
                StateHasChanged();
                return;
            }
            if(livestockType is not null)
                livestockTypeId=livestockType.Id;

            livestockStatus= new LivestockStatusModel() { LivestockTypeId = livestockTypeId.Value };
            var query = dbContext.LivestockStatuses.AsQueryable();
            if (livestockStatusId.HasValue && livestockStatusId > 0)
                query = query.Where(f => f.Id == livestockStatusId);
            if (livestockTypeId.HasValue && livestockTypeId > 0)
                query = query.Where(f => f.LivestockTypeId == livestockTypeId);
            
            if(!createOnly)
                livestockStatus = await query.OrderBy(f => f.Id).FirstOrDefaultAsync() ?? new LivestockStatusModel() { LivestockTypeId=livestockTypeId.Value };
            
            editContext = new EditContext(livestockStatus);
        }
        public override async Task OnSubmit()
        {
            try
            {
                _submitting = true;
                var state = livestockStatus.Id <= 0 ? EntityState.Added : EntityState.Modified;

                if (livestockStatus.Id <= 0)
                    livestockStatus.Id = await api.ProcessCommand<LivestockStatusModel, CreateLivestockStatus>("api/CreateLivestockStatus", new CreateLivestockStatus { LivestockStatus = livestockStatus });
                else
                    livestockStatus.Id = await api.ProcessCommand<LivestockStatusModel, UpdateLivestockStatus>("api/UpdateLivestockStatus", new UpdateLivestockStatus { LivestockStatus = livestockStatus });

                if (livestockStatus.Id <= 0)
                    throw new Exception("Failed to save livestock Status");

                editContext = new EditContext(livestockStatus);
                await Submitted.InvokeAsync(livestockStatus);
                _submitting = false;
                if (createOnly)
                {
                    livestockStatus = new LivestockStatusModel() { LivestockTypeId = livestockTypeId.Value };
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
