using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.Milestone;
using FrontEnd.Components.Duty;

namespace FrontEnd.Components.Milestone
{
    public partial class MilestoneEditor : DataComponent<MilestoneModel>
    {
        [CascadingParameter] public LivestockAnimalSummary LivestockAnimal { get; set; }
        [CascadingParameter] public MilestoneModel Milestone { get; set; }
        [Parameter] public long? milestoneId { get; set; }
        private ValidatedForm _validatedForm;

        protected new MilestoneModel working { get => base.working as MilestoneModel; set { base.working = value; } }
        public override async Task FreshenData()
        {
            var recipientType=string.Empty;
            var recipientTypeId = 0L;
            if (LivestockAnimal is not null)
            {
                recipientTypeId = LivestockAnimal.Id;
                recipientType = LivestockAnimal.EntityName;
            }
               

            working = new MilestoneModel() { RecipientType= recipientType,RecipientTypeId = recipientTypeId};

            if (Milestone is not null)
                working = Milestone;

            if (Milestone is null && milestoneId > 0)
                working = await app.dbContext.Milestones.FindAsync(milestoneId);

            SetEditContext(working);
        }
        public async Task OnSubmit()
        {
            try
            {

               long id = (working.Id <= 0)?
                    working.Id = await app.api.ProcessCommand<MilestoneModel, CreateMilestone>("api/CreateMilestone", new CreateMilestone { Milestone = working }):
                    working.Id = await app.api.ProcessCommand<MilestoneModel, UpdateMilestone>("api/UpdateMilestone", new UpdateMilestone { Milestone = working  });

                if (id <= 0)
                    throw new Exception("Failed to save livestock Status");

                working.Id = id;
                SetEditContext(working);
                await Submitted.InvokeAsync(working);
            }
            catch (Exception ex)
            {

            }
        }

        private DutyEditor _dutyEditor;
        private bool showDutyModal = false;
        
        private void ShowDutyEditor()
        {
            showDutyModal = true;
            StateHasChanged();
        }
        private void DutyCreated(object e)
        {
            var model = e as DutyModel;
            showDutyModal = false;
            working.Duties.Add(model);
            StateHasChanged();
        }
        private void DutyCanceled()
        {
            showDutyModal = false;
            StateHasChanged();
        }
        void DutySelected(ChangeEventArgs e)
        {
            working.Duties.Add(app.dbContext.Duties.Find(long.Parse(e.Value.ToString())));
            StateHasChanged();
        }

        void DutyRemoved(DutyModel duty)
        {
            working.Duties.Remove(duty);
            StateHasChanged();
        }

        private async Task Cancel()
        {
            working=((MilestoneModel)original).Map(working) as MilestoneModel;
            SetEditContext(working);
            await Cancelled.InvokeAsync(working);

        }
    }
}
