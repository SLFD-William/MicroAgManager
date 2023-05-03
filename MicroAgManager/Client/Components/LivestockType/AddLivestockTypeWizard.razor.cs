using Domain.Constants;
using Domain.Entity;
using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Data;
using FrontEnd.Persistence;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.LivestockType
{
    public partial class AddLivestockTypeWizard
    {
        private const string Step1 = "Type Definition";
        private const string Step2 = "Feeding";
        private const string Step3 = "Main Plot";
        private Wizard? wizard;
        [CascadingParameter] public LivestockTypeModel livestockType { get; set; }
        [CascadingParameter] DataSynchronizer dbSync { get; set; }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        [Parameter] public EventCallback<bool> Completed { get; set; }
        [Parameter] public bool IsNestedWizard { get; set; } = false;


        private LivestockTypeEditor? livestockTypeEditor;

        private void LivestockFeedWizardCompleted(bool args)
        {
            if (args)
                wizard?.GoNext();
            else
                wizard?.GoBack();
        }

        private bool _buttonsVisible = true;
        private Task<bool> ShowButtons()
        {
            if (string.IsNullOrEmpty(wizard?.ActiveStep?.Name)) return Task.FromResult( _buttonsVisible);
            _buttonsVisible = true;
            if (wizard.ActiveStep.Name == Step2)
                _buttonsVisible = false;
            return Task.FromResult(_buttonsVisible);
        }
        protected async override Task OnInitializedAsync()
        {
            if(dbContext is null) dbContext = await dbSync.GetPreparedDbContextAsync();
            if (livestockType is null) livestockType = new();
            StateHasChanged();
        }
       
        private async Task<bool> CanStepAdvance()
        {
            if (wizard?.ActiveStep?.Name == Step1)
                if (livestockTypeEditor is not null && livestockTypeEditor.editContext.IsModified())
                {
                    if (!livestockTypeEditor.editContext.Validate()) return false;
                    await livestockTypeEditor.OnSubmit();
                }
            //if (ActiveStep.Name == Step2)
            //    if (landPlotEditor is not null && landPlotEditor.editContext.IsModified())
            //    {
            //        if (!landPlotEditor.editContext.Validate()) return false;
            //        await landPlotEditor.OnSubmit();
            //    }
            return true;
        }
        private async Task<bool> CanStepRetreat()
        {
            if (wizard?.ActiveStep?.Name == Step1 && IsNestedWizard)
                await Completed.InvokeAsync(false);
               
            return true;
        }
        private void LivestockTypeUpdated(LivestockTypeModel args) => livestockType = args;
    }
}
