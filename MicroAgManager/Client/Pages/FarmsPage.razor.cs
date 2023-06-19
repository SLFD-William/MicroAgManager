using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace FrontEnd.Pages
{
    public partial class FarmsPage : DataComponent
    {
        [Parameter] public long? farmId { get; set; }
        [Inject] public  IJSRuntime js { get; set; }
        public bool multipleFarms() => app.dbContext?.Farms.Count() > 1;
        private FarmLocationModel selectedFarm { get; set; }
        private async Task Cancelled() {
            farmId = null;
            await js.InvokeVoidAsync("goBack");
        } 
        private async Task FarmLocationUpdated(FarmLocationModel args)
        {
            while (!app.dbContext.Farms.Any(t => t.Id == args.Id))
                await Task.Delay(100);

            farmId = null;
            await js.InvokeVoidAsync("goBack");
        }
        public override async Task FreshenData()
        {
            var query = app.dbContext.Farms.AsQueryable();
            if (selectedFarm is null)
                selectedFarm=await query.FirstOrDefaultAsync() ?? new FarmLocationModel();
            else
                selectedFarm = await query.SingleAsync(x=>x.Id==selectedFarm.Id);
        }
    }
}
