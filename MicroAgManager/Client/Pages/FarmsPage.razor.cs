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
            if (selectedFarm.Id >0)
                await js.InvokeVoidAsync("goBack"); 
            await FreshenData();
        } 
        private async Task FarmLocationUpdated(FarmLocationModel args)
        {
            while (!app.dbContext.Farms.Any(t => t.Id == args.Id))
                await Task.Delay(100);
            if (selectedFarm.Id > 0)
                await js.InvokeVoidAsync("goBack");
            farmId = null;
            await FreshenData();
        }
        public override async Task FreshenData()
        {
            if (selectedFarm is null || selectedFarm.Id<1)
                selectedFarm=await app.dbContext.Farms.FirstOrDefaultAsync() ?? new FarmLocationModel();
            else
                selectedFarm = await app.dbContext.Farms.FindAsync(selectedFarm.Id) ?? new FarmLocationModel();
        }
    }
}
