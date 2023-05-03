using BackEnd.BusinessLogic.Livestock.Status;
using BackEnd.BusinessLogic.Livestock.Types;
using Domain.Constants;
using Domain.Entity;
using Domain.Models;
using FrontEnd.Data;
using FrontEnd.Persistence;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockStatus
{
    public partial class LivestockStatusEditor:ComponentBase
    {
        private readonly static List<string> StatusModes = new List<string> {
            string.Empty,
            LivestockStatusModeConstants.Unchanged,
            LivestockStatusModeConstants.False,
            LivestockStatusModeConstants.True
        };
        [CascadingParameter] IFrontEndApiServices api { get; set; }
        [CascadingParameter] DataSynchronizer dbSync { get; set; }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        [CascadingParameter] public LivestockTypeModel livestockType { get; set; }
        [Parameter] public EventCallback<LivestockStatusModel> Submitted { get; set; }
        [Parameter] public long? livestockTypeId { get; set; }
        [Parameter] public long? livestockStatusId { get; set; }

        public EditContext editContext { get; private set; }
        private LivestockStatusModel livestockStatus;
        protected async override Task OnInitializedAsync()
        {
            dbSync.OnUpdate += DbSync_OnUpdate;
            await FreshenData();
        }

        private void DbSync_OnUpdate() => Task.Run(FreshenData);

        private async Task FreshenData()
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

            var query = dbContext.LivestockStatuses.AsQueryable();
            if (livestockStatusId.HasValue && livestockStatusId > 0)
                query = query.Where(f => f.Id == livestockStatusId);
            if (livestockTypeId.HasValue && livestockTypeId > 0)
                query = query.Where(f => f.LivestockTypeId == livestockTypeId);

            livestockStatus = await query.OrderBy(f => f.Id).FirstOrDefaultAsync() ?? new LivestockStatusModel() { LivestockTypeId=livestockTypeId.Value };
            editContext = new EditContext(livestockStatus);
        }
        public async Task OnSubmit()
        {
            try
            {
                var state = livestockStatus.Id <= 0 ? EntityState.Added : EntityState.Modified;

                if (livestockStatus.Id <= 0)
                    livestockStatus.Id = await api.ProcessCommand<LivestockStatusModel, CreateLivestockStatus>("api/CreateLivestockStatus", new CreateLivestockStatus { LivestockStatus = livestockStatus });
                else
                    livestockStatus.Id = await api.ProcessCommand<LivestockStatusModel, UpdateLivestockStatus>("api/UpdateLivestockStatus", new UpdateLivestockStatus { LivestockStatus = livestockStatus });

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
        public ValueTask DisposeAsync()
        {
            dbSync.OnUpdate -= DbSync_OnUpdate;
            return ValueTask.CompletedTask;
        }
    }
}
