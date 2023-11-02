using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.Livestock
{
    public partial class LivestockViewer : DataComponent<LivestockModel>
    {
        [CascadingParameter] public LivestockModel? Livestock { get; set; }
        [Parameter] public long? livestockId { get; set; }
        private LivestockModel livestock { get; set; } = new LivestockModel();

        private bool _editting = false;
        private void ShowEditor() => _editting = true;
        public override async Task FreshenData()
        {
            if (Livestock is not null)
                livestock = Livestock;
            if (Livestock is null && livestockId.HasValue)
                livestock = await app.dbContext.Livestocks.FindAsync(livestockId);
            StateHasChanged();
        }
        private async Task EditComplete(bool submit = false)
        {
            _editting = false;
            await FreshenData();
            if (submit) await Submitted.InvokeAsync(Livestock);
            else await Cancelled.InvokeAsync();
        }

    }
}
