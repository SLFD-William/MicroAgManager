using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.Milestone;
using FrontEnd.Components.Duty;

namespace FrontEnd.Components.Milestone
{
    public partial class MilestoneEditor : DataComponent
    {
        [CascadingParameter] public LivestockAnimalSummary LivestockAnimal { get; set; }
        [CascadingParameter] public MilestoneModel Milestone { get; set; }
        [Parameter] public long? milestoneId { get; set; }
        private MilestoneModel milestone { get; set; }
        private ValidatedForm _validatedForm;
        private List<DutyModel?> originalDuties;
        protected async override Task OnParametersSetAsync() 
        {
            await FreshenData();
            originalDuties = milestone.Duties.ToList();
        }
        

        public override async Task FreshenData()
        {
            var recipientType=string.Empty;
            var recipientTypeId = 0L;
            if (LivestockAnimal is not null)
            {
                recipientTypeId = LivestockAnimal.Id;
                recipientType = LivestockAnimal.EntityName;
            }
               

            milestone = new MilestoneModel() { RecipientType= recipientType,RecipientTypeId = recipientTypeId};

            if (Milestone is not null)
                milestone = Milestone;

            if (Milestone is null && milestoneId > 0)
                milestone = await app.dbContext.Milestones.FindAsync(milestoneId);

            editContext = new EditContext(milestone);
        }
        public async Task OnSubmit()
        {
            try
            {

               long id = (milestone.Id <= 0)?
                    milestone.Id = await app.api.ProcessCommand<MilestoneModel, CreateMilestone>("api/CreateMilestone", new CreateMilestone { Milestone = milestone }):
                    milestone.Id = await app.api.ProcessCommand<MilestoneModel, UpdateMilestone>("api/UpdateMilestone", new UpdateMilestone { Milestone = milestone });

                if (id <= 0)
                    throw new Exception("Failed to save livestock Status");

                milestone.Id = id;
                originalDuties = milestone.Duties.ToList();
                editContext = new EditContext(milestone);
                await Submitted.InvokeAsync(milestone);
                StateHasChanged();
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
            milestone.Duties.Add(model);
            editContext = new EditContext(milestone);
            StateHasChanged();
        }
        private void DutyCanceled()
        {
            showDutyModal = false;
            StateHasChanged();
        }
        void DutySelected(ChangeEventArgs e)
        {
            milestone.Duties.Add(app.dbContext.Duties.Find(long.Parse(e.Value.ToString())));
            editContext = new EditContext(milestone);
            StateHasChanged();
        }

        void DutyRemoved(DutyModel duty)
        {
            milestone.Duties.Remove(duty);
            editContext = new EditContext(milestone);
            StateHasChanged();
        }

        private async Task Cancel()
        {
            milestone.Duties = originalDuties;
            editContext = new EditContext(milestone);
            await Cancelled.InvokeAsync(milestone);
            StateHasChanged();
        }
    }
}
