using Domain.Models;
using FrontEnd.Components.Livestock;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockBreed
{
    public partial class LivestockBreedSubTabs : DataComponent
    {
        [CascadingParameter] public LivestockBreedModel? LivestockBreed { get; set; }
        [Parameter] public long? livestockBreedId { get; set; }
        private LivestockBreedModel livestockBreed { get; set; } = new LivestockBreedModel();
        protected TabControl _tabControl;
        protected TabPage _livestockTab;

        private LivestockEditor? _livestockEditor;
        protected LivestockList _livestockList;
        protected override void OnInitialized()
        {
            _tabControl?.ActivatePage(app.SelectedTabs[nameof(LivestockBreedSubTabs)] ?? _tabControl?.ActivePage ?? _livestockTab);
        }
        public override async Task FreshenData()
        {
            if (LivestockBreed is not null)
                livestockBreed = await app.dbContext.LivestockBreeds.FindAsync(LivestockBreed.Id) ?? new LivestockBreedModel();
            else
            {
                var query = app.dbContext?.LivestockBreeds.AsQueryable();
                if (livestockBreedId.HasValue && livestockBreedId > 0)
                    query = query.Where(f => f.Id == livestockBreedId);
                livestockBreed = await query.OrderBy(f => f.Id).SingleOrDefaultAsync() ?? new LivestockBreedModel();
            }
            if (_livestockList is not null)
                await _livestockList.FreshenData();
        }
        private async Task LivestockUpdated(object args)
        {
            var model=args as LivestockModel;

            if (model?.Id > 0)
                while(!app.dbContext.Livestocks.Any(t => t.Id == model.Id))
                    await Task.Delay(100);

            await FreshenData();
        }
    }
}
