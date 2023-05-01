using Domain.Constants;
using Domain.Entity;
using Domain.Models;
using FrontEnd.Components.LivestockFeed;
using FrontEnd.Persistence;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.LivestockType
{
    public partial class AddLivestockTypeWizard
    {
        enum Steps
        {
            TypeDefinition = 1,
            LivestockFeed = 2,
            LivestockChore = 3,
        }
        [CascadingParameter] public LivestockTypeModel livestockType { get; set; }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        [Parameter] public EventCallback<bool> Completed { get; set; }
        public int Step { get; set; } = 1;
        
        private LivestockTypeEditor? livestockTypeEditor;

        private LivestockFeedEditor? livestockFeedEditor;
        private async Task LivestockFeedWizardCompleted(bool args)
        {
            if (args) 
                Step = await ProcessStepOnNextTransition();
            else
                Prev();
        }
        protected async override Task OnInitializedAsync()
        {
            if (dbContext is null) return;
            if(livestockType is null) livestockType = new();
            StateHasChanged();
        }
        private bool ShowPrevious()
        {
            var show = Step != 1;
            if (Step == (int)Steps.LivestockFeed)
                show = false;

            return show;
        }
        private bool ShowNext()
        {
            var show = Step != 4;
            if (Step == (int)Steps.LivestockFeed)
                show = false;
            return show;
        }
        private async Task Next()
        {
            Step = await ProcessStepOnNextTransition();
            if (Step > 3) await Completed.InvokeAsync(true);
            StateHasChanged();
        }
        private void Prev()
        {
            Step--;
            if(Step<1) Completed.InvokeAsync(false);
            StateHasChanged();
        }
        private void LivestockTypeUpdated(LivestockTypeModel args) => livestockType = args;
        private async Task<int> ProcessStepOnNextTransition()
        {
            switch (Step)
            {
                case (int)Steps.TypeDefinition:
                    if (livestockTypeEditor is not null && livestockTypeEditor.editContext.IsModified())
                    {
                        if (!livestockTypeEditor.editContext.Validate()) return (int)Steps.TypeDefinition;
                        await livestockTypeEditor.OnSubmit();
                    }
                    return (int)Steps.LivestockFeed;
                case (int)Steps.LivestockFeed:
                    if (livestockFeedEditor is not null && livestockFeedEditor.editContext.IsModified())
                    {
                        if (!livestockFeedEditor.editContext.Validate()) return (int)Steps.LivestockFeed;
                        await livestockFeedEditor.OnSubmit();
                    }
                    return (int)Steps.LivestockFeed;

                    //case (int)Steps.MainPlot:
                    //    if (landPlotEditor is not null && landPlotEditor.editContext.IsModified())
                    //    {
                    //        if (!landPlotEditor.editContext.Validate()) return (int)Steps.MainPlot;
                    //        await landPlotEditor.OnSubmit();
                    //    }
                    //    return (int)Steps.FarmProduction;
            }
            return Step + 1;
        }
    }
}
