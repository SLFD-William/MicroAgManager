using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockAnimal
{
    public partial class LivestockAnimalViewer : DataComponent<LivestockAnimalModel>
    {
        [CascadingParameter] public LivestockAnimalModel? LivestockAnimal { get; set; }
        [Parameter] public long? livestockAnimalId { get; set; }

        private LivestockAnimalModel livestockAnimal { get; set; } = new LivestockAnimalModel();

        [Parameter] public EventCallback<LivestockAnimalModel> Submitted { get; set; }
        [Parameter] public EventCallback Cancelled { get; set; }
        private bool _editting = false;
        private void ShowEditor() => _editting = true;

        private async Task EditComplete(bool submit = false)
        {
            _editting = false;
            await FreshenData();
            if (submit) await Submitted.InvokeAsync(livestockAnimal);
            else await Cancelled.InvokeAsync();

        }
        public override async Task FreshenData()
        {
            if (LivestockAnimal is not null)
            {
                livestockAnimal = LivestockAnimal;
                StateHasChanged();
                return;
            }
            var query = app.dbContext?.LivestockAnimals.AsQueryable();
            if (livestockAnimalId.HasValue && livestockAnimalId > 0)
                query = query.Where(f => f.Id == livestockAnimalId);
            livestockAnimal = await query.OrderBy(f => f.Id).SingleOrDefaultAsync() ?? new LivestockAnimalModel();
            StateHasChanged();
        }
    }
}
