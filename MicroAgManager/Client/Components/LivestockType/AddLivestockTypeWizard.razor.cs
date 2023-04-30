using Domain.Models;
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
        }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        
        public int Step { get; set; } = 1;
        
        private LivestockTypeEditor? livestockTypeEditor;

        protected LivestockTypeModel livestockType;
        protected async override Task OnInitializedAsync()
        {
            if (dbContext is null) return;
            if(livestockType is null) livestockType = new();
            StateHasChanged();
        }
        private async Task Next()
        {
            Step = await ProcessStepOnNextTransition();
            StateHasChanged();
        }
        private void Prev()
        {
            Step--;
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
