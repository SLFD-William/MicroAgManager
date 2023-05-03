using BackEnd.BusinessLogic.LivestockFeed;
using Domain.Models;
using FrontEnd.Data;
using FrontEnd.Persistence;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockFeed
{
    public partial class LivestockFeedEditor : IAsyncDisposable
    {
        [CascadingParameter] IFrontEndApiServices api { get; set; }
        [CascadingParameter] DataSynchronizer dbSync { get; set; }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }

        [CascadingParameter] public LivestockTypeModel livestockType { get; set; }
        [Parameter] public EventCallback<LivestockFeedModel> Submitted { get; set; }
        [Parameter] public long? livestockFeedId { get; set; }
        [Parameter] public LivestockFeedModel livestockFeed { get; set; }
        public EditContext editContext { get; private set; }
        protected async override Task OnInitializedAsync()
        {
            dbSync.OnUpdate += DbSync_OnUpdate;
            await FreshenData();
        }

        private void DbSync_OnUpdate() => Task.Run(FreshenData);

        private async Task FreshenData()
        {
            if(dbContext is null) dbContext = await dbSync.GetPreparedDbContextAsync();
            if (livestockFeed is not null)
            {
                editContext = new EditContext(livestockFeed);
                return;
            }
            var query = dbContext.LivestockFeeds.AsQueryable();
            if (livestockFeedId.HasValue && livestockFeedId > 0)
                query = query.Where(f => f.Id == livestockFeedId);
            livestockFeed = await query.OrderBy(f => f.Id).FirstOrDefaultAsync() ?? new LivestockFeedModel{ LivestockTypeId=livestockType.Id };
            editContext = new EditContext(livestockType);
        }
        public async Task OnSubmit()
        {
            try
            {
                if (livestockFeed.Id <= 0)
                    livestockFeed.Id = await api.ProcessCommand<LivestockFeedModel, CreateLivestockFeed>("api/CreateLivestockFeed", new CreateLivestockFeed { LivestockFeed = livestockFeed });
                else
                    livestockFeed.Id = await api.ProcessCommand<LivestockFeedModel, UpdateLivestockFeed>("api/UpdateLivestockFeed", new UpdateLivestockFeed { LivestockFeed = livestockFeed });

                if (livestockFeed.Id <= 0)
                    throw new Exception("Failed to save livestock feed");

                editContext = new EditContext(livestockFeed);
                await Submitted.InvokeAsync(livestockFeed);
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
