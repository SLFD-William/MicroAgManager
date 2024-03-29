﻿using Domain.Models;
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

            SetEditContext((MilestoneModel)working);
        }
        public async Task OnSubmit()
        {
            try
            {

               long id = (((MilestoneModel)working).Id <= 0)?
                    ((MilestoneModel)working).Id = await app.api.ProcessCommand<MilestoneModel, CreateMilestone>("api/CreateMilestone", new CreateMilestone { Milestone = (MilestoneModel)working }):
                    ((MilestoneModel)working).Id = await app.api.ProcessCommand<MilestoneModel, UpdateMilestone>("api/UpdateMilestone", new UpdateMilestone { Milestone = (MilestoneModel)working  });

                if (id <= 0)
                    throw new Exception("Failed to save livestock Status");

                ((MilestoneModel)working).Id = id;
                SetEditContext((MilestoneModel)working);
                await Submitted.InvokeAsync((MilestoneModel)working);
            }
            catch (Exception ex)
            {

            }
        }

        private DutyEditor _dutyEditor;
        private DutyModel? _editDuty;
        private bool showDutyModal = false;
        
        private void ShowDutyEditor()
        {
            _editDuty=new DutyModel();
            showDutyModal = true;
            StateHasChanged();
        }
        private void DutyCreated(object e)
        {
            var model = e as DutyModel;
            showDutyModal = false;
            ((MilestoneModel)working).Duties.Add(model);
            _editDuty = null;
            StateHasChanged();
        }
        private void DutyCanceled()
        {
            showDutyModal = false;
            _editDuty = null;
            StateHasChanged();
        }
        void DutySelected(ChangeEventArgs e)
        {
            ((MilestoneModel)working).Duties.Add(app.dbContext.Duties.Find(long.Parse(e.Value.ToString())));
            StateHasChanged();
        }

        void DutyRemoved(DutyModel duty)
        {
            ((MilestoneModel)working).Duties.Remove(duty);
            StateHasChanged();
        }

        private async Task Cancel()
        {
            working=((MilestoneModel)original).Map((MilestoneModel)working) ;
            SetEditContext((MilestoneModel)working);
            await Cancelled.InvokeAsync(working);

        }
    }
}
