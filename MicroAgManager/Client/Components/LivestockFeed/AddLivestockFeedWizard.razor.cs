using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.LivestockFeed
{
    public partial class AddLivestockFeedWizard
    {
        private Wizard? wizard;
        private const string Step1 = "Feed Definition";
        private const string Step2 = "Servings";
        [Inject] ApplicationStateProvider app { get; set; }
        [CascadingParameter] public LivestockTypeModel livestockType { get; set; }
        [Parameter] public EventCallback<bool> Completed { get; set; }
        [Parameter] public bool IsNestedWizard { get; set; } = false;
        public int Step { get; set; } = 1;
        private LivestockFeedEditor? livestockFeedEditor;

        protected LivestockFeedModel livestockFeed;
        protected async override Task OnInitializedAsync()
        {
            await FreshenData();
        }


        private async Task FreshenData()
        {
            if (livestockFeed is null) livestockFeed = new LivestockFeedModel { LivestockTypeId = livestockType.Id };
            StateHasChanged();
        }
        private async Task<bool> CanStepAdvance()
        {
            if (wizard?.ActiveStep?.Name == Step1)
                if (livestockFeedEditor is not null && livestockFeedEditor.editContext.IsModified())
                {
                    if (!livestockFeedEditor.editContext.Validate()) return false;
                    await livestockFeedEditor.OnSubmit();
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
        private void LivestockFeedUpdated(LivestockFeedModel args) => livestockFeed = args;
    }
}
