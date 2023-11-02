using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.Milestone
{
    public partial class MilestoneList : DataComponent<MilestoneModel>
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
                    .Where(f => f.RecipientTypeId == LivestockAnimal.Id && f.RecipientType==LivestockAnimal.EntityName).OrderBy(f => f.EntityModifiedOn)
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
            _editMilestone = id > 0 ? await FindMilestone(id) : new MilestoneModel { RecipientTypeId = LivestockAnimal.Id, RecipientType=LivestockAnimal.EntityName };
            StateHasChanged();
        }
        private async Task EditCancelled()
        {
            _editMilestone = null;
            StateHasChanged();
        }
        private async Task MilestoneUpdated(object args)
        {
            var model=args as MilestoneModel;
            if (model?.Id > 0)
                while (!app.dbContext.Milestones.Any(t => t.Id == model.Id))
                    await Task.Delay(100);

            _editMilestone = null;
            await Submitted.InvokeAsync(await FindMilestone(model.Id));
        }
    }
}
