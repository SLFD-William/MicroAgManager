using BackEnd.BusinessLogic.LivestockFeed;
using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockFeed
{
    public partial class LivestockFeedEditor : Editor<LivestockFeedModel>
    {

        [CascadingParameter] public LivestockAnimalModel LivestockAnimal { get; set; }
        [Parameter] public long? livestockFeedId { get; set; }
        [Parameter] public LivestockFeedModel livestockFeed { get; set; }

        public override async Task FreshenData()
        {
            if (livestockFeed is not null)
            {
                editContext = new EditContext(livestockFeed);
                return;
            }
            var query = app.dbContext.LivestockFeeds.AsQueryable();
            if (livestockFeedId.HasValue && livestockFeedId > 0)
                query = query.Where(f => f.Id == livestockFeedId);
            livestockFeed = await query.OrderBy(f => f.Id).FirstOrDefaultAsync() ?? new LivestockFeedModel{ LivestockAnimalId=LivestockAnimal.Id };
            editContext = new EditContext(LivestockAnimal);
        }
        private async void Cancel()
        {
            editContext = new EditContext(livestockFeed);
            await Cancelled.InvokeAsync();
            StateHasChanged();
        }
        public override async Task OnSubmit()
        {
            try
            {
                if (livestockFeed.Id <= 0)
                    livestockFeed.Id = await app.api.ProcessCommand<LivestockFeedModel, CreateLivestockFeed>("api/CreateLivestockFeed", new CreateLivestockFeed { LivestockFeed = livestockFeed });
                else
                    livestockFeed.Id = await app.api.ProcessCommand<LivestockFeedModel, UpdateLivestockFeed>("api/UpdateLivestockFeed", new UpdateLivestockFeed { LivestockFeed = livestockFeed });

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
    }
}
