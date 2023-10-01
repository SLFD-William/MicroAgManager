using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using BackEnd.BusinessLogic.Milestone;

namespace FrontEnd.Components.Milestone
{
    public partial class MilestoneEditor : DataComponent
    {
        [CascadingParameter] public LivestockAnimalSummary LivestockAnimal { get; set; }
        [CascadingParameter] public MilestoneModel Milestone { get; set; }
        [Parameter] public bool showUpdateCancelButtons { get; set; }
        [Parameter] public EditContext editContext { get; set; }
        [Parameter] public EventCallback<MilestoneModel> Submitted { get; set; }
        [Parameter] public EventCallback Cancelled { get; set; }
        [Parameter] public long? livestockAnimalId { get; set; }
        [Parameter] public long? milestoneId { get; set; }
        private MilestoneModel milestone { get; set; }
        private ValidatedForm _validatedForm;
        [Parameter] public bool Modal { get; set; }
        protected override async Task OnInitializedAsync() => await FreshenData();

        public override async Task FreshenData()
        {
            if (Milestone is not null)
            {
                milestone = Milestone;
                editContext = new EditContext(milestone);
                StateHasChanged();
                return;
            }
            if (LivestockAnimal is not null)
                livestockAnimalId = LivestockAnimal.Id;

            var query = app.dbContext.Milestones.AsQueryable();
            if (milestoneId.HasValue && milestoneId > 0)
                query = query.Where(f => f.Id == milestoneId);
            if (livestockAnimalId.HasValue && livestockAnimalId > 0)
                query = query.Where(f => f.LivestockAnimalId == livestockAnimalId);

            milestone = await query.FirstOrDefaultAsync() ?? new MilestoneModel() { LivestockAnimalId = livestockAnimalId.Value }; ;

            editContext = new EditContext(milestone);
        }
        public async Task OnSubmit()
        {
            try
            {

                var state = milestone.Id <= 0 ? EntityState.Added : EntityState.Modified;

                if (milestone.Id <= 0)
                    milestone.Id = await app.api.ProcessCommand<MilestoneModel, CreateMilestone>("api/CreateMilestone", new CreateMilestone { Milestone = milestone });
                else
                    milestone.Id = await app.api.ProcessCommand<MilestoneModel, UpdateMilestone>("api/UpdateMilestone", new UpdateMilestone { Milestone = milestone });

                if (milestone.Id <= 0)
                    throw new Exception("Failed to save livestock Status");

                editContext = new EditContext(milestone);
                await Submitted.InvokeAsync(milestone);
                _validatedForm.HideModal();
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
        private async Task Cancel()
        {
            editContext = new EditContext(milestone);
            await Cancelled.InvokeAsync(milestone);
            _validatedForm.HideModal();
            StateHasChanged();
        }
    }
}
