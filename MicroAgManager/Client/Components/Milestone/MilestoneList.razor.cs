using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.Milestone
{
    public partial class MilestoneList : DataComponent
    {
        protected TableTemplate<MilestoneSummary> _listComponent;
        [CascadingParameter] LivestockAnimalSummary LivestockAnimal { get; set; }
        [Parameter] public IEnumerable<MilestoneSummary>? Items { get; set; }
        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<MilestoneModel>? MilestoneSelected { get; set; }

        private MilestoneModel? _editMilestone;
        private MilestoneEditor? _milestoneEditor;
        protected override void  OnInitialized()
        {
            if (!app.RowDetailsShowing.ContainsKey("MilestoneList"))
                app.RowDetailsShowing.Add("MilestoneList", new List<object>());
        }
        public override async Task FreshenData()
        {

            if (Items is null)
                Items = (await app.dbContext.Milestones
                    .Where(f => f.LivestockAnimalId == LivestockAnimal.Id).OrderBy(f => f.EntityModifiedOn)
                    .Select(s => new MilestoneSummary(s, app.dbContext)).ToListAsync()).AsEnumerable();

            _listComponent?.Update();
        }
        private async Task<MilestoneModel?> FindMilestone(long Id) => await app.dbContext.Milestones.FindAsync(Id);
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                MilestoneSelected?.Invoke(Task.Run(async () => await FindMilestone(_listComponent.SelectedItems.First().Id)).Result);
        }
        private async Task EditMilestone(long id)
        {
            _editMilestone = id > 0 ? await FindMilestone(id) : new MilestoneModel { LivestockAnimalId = LivestockAnimal.Id };
            StateHasChanged();
        }
        private async Task EditCancelled()
        {
            _editMilestone = null;
            await FreshenData();
        }
        private async Task MilestoneUpdated(MilestoneModel args)
        {
            if (args.Id > 0)
                while (!app.dbContext.Milestones.Any(t => t.Id == args.Id))
                    await Task.Delay(100);

            _editMilestone = null;
            await FreshenData();
        }
    }
}
