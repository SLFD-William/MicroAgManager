using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.Farm
{
    public partial class FarmViewer :DataComponent<FarmLocationModel>
    {
        [CascadingParameter] public FarmLocationModel? FarmLocation { get; set; }
        [Parameter] public long? farmId { get; set; }

        private FarmLocationModel farm { get; set; } = new FarmLocationModel();
        private bool _editting=false;
        private void ShowEditor()=>_editting = true;
        
        private async Task EditComplete(bool submit=false)
        {
            _editting = false;
            await FreshenData();
            if(submit) await Submitted.InvokeAsync(farm);
            else await Cancelled.InvokeAsync();

        }
        public override async Task FreshenData()
        {
            if (FarmLocation is not null)
            {
                farm = FarmLocation;
                StateHasChanged();
                return;
            }
            var query = app.dbContext?.Farms.AsQueryable();
            if (farmId.HasValue && farmId > 0)
                query = query.Where(f => f.Id == farmId);
            farm = await query.OrderBy(f => f.Id).SingleOrDefaultAsync() ?? new FarmLocationModel();
            StateHasChanged();
        }
    }
}
