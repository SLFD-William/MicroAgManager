using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.Farm
{
    public partial class FarmViewer :DataComponent<FarmLocationModel>
    {
        [CascadingParameter] public FarmLocationModel? FarmLocation { get; set; }
        [Parameter] public long? farmId { get; set; }
        private Weather.Weather _weather;
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
                farm = FarmLocation;
            if (FarmLocation is null && farmId.HasValue)
                farm = await app.dbContext.Farms.FindAsync(farmId);

            if(_weather is not null)
                await _weather.RefreshWeather();

            StateHasChanged();
        }
    }
}
