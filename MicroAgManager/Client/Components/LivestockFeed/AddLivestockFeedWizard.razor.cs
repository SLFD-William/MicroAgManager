using Domain.Entity;
using Domain.Models;
using FrontEnd.Components.LivestockType;
using FrontEnd.Persistence;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Components.LivestockFeed
{
    public partial class AddLivestockFeedWizard
    {
        enum Steps
        {
            FeedDefinition = 1,
            CreateServings=2,
            CreateAnalysis=3,
            Complete=4
        }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        [CascadingParameter] public LivestockTypeModel livestockType { get; set; }
        [Parameter] public EventCallback<bool> Completed { get; set; }
        public int Step { get; set; } = 1;
        private LivestockFeedEditor? livestockFeedEditor;

        protected LivestockFeedModel livestockFeed;
        protected async override Task OnInitializedAsync()
        {
            if (dbContext is null) return;
            if (livestockFeed is null) livestockFeed = new LivestockFeedModel { LivestockTypeId = livestockType.Id };
            StateHasChanged();
        }
        private bool ShowPrevious()
        { return Step != 0; }
        private bool ShowNext()
        { return Step != 4; }
        private async Task Next()
        {
            Step = await ProcessStepOnNextTransition();
            if (Step > 3) await Completed.InvokeAsync(true);
            StateHasChanged();
        }
        private void Prev()
        {
            Step--;
            if (Step < 1) Completed.InvokeAsync(false);
            StateHasChanged();
        }
        private void LivestockFeedUpdated(LivestockFeedModel args) => livestockFeed = args;
        private async Task<int> ProcessStepOnNextTransition()
        {
            switch (Step)
            {
                case (int)Steps.FeedDefinition:
                    if (livestockFeedEditor is not null && livestockFeedEditor.editContext.IsModified())
                    {
                        if (!livestockFeedEditor.editContext.Validate()) return (int)Steps.FeedDefinition;
                        await livestockFeedEditor.OnSubmit();
                    }
                    return (int)Steps.CreateServings;
                    //case (int)Steps.LivestockFeed:
                    //    if (livestockFeedEditor is not null && livestockFeedEditor.editContext.IsModified())
                    //    {
                    //        if (!livestockFeedEditor.editContext.Validate()) return (int)Steps.LivestockFeed;
                    //        await livestockFeedEditor.OnSubmit();
                    //    }
                    //    return (int)Steps.LivestockFeed;

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
