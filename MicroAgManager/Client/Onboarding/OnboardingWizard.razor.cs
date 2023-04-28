using Domain.Models;
using FrontEnd.Data;
using FrontEnd.Persistence;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using FrontEnd.Components.Farm;
using FrontEnd.Components.LandPlot;

namespace FrontEnd.Onboarding
{
    public partial class OnboardingWizard
    {
        enum Steps
        { 
            FarmLocation =1,
            MainPlot=2,
            FarmProduction =3,
            LiveStockDefinition=10,
            LiveStockFeed = 11,
            LiveStockFeedAnalysis = 12,
            LiveStockFeedServing = 13,
            LiveStockFeedChore = 13,
        }

        [CascadingParameter] DataSynchronizer dbSync { get; set; }
        public int Step { get; set; } = 1;
        private FrontEndDbContext dbContext { get; set; }

        protected TenantModel tenant;

        private FarmEditor? farmEditor;
        protected FarmLocationModel farm=new();
        
        private LandPlotEditor? landPlotEditor;
        protected LandPlotModel landPlot = new();
        private void FarmLocationUpdated(FarmLocationModel args)=>farm = args;
        private void LandPlotUpdated(LandPlotModel args) => landPlot = args;

        protected async override Task OnInitializedAsync()
        {
            if (dbSync is null) return;
            dbContext = await dbSync.GetPreparedDbContextAsync();
            tenant = await dbContext.Tenants.SingleAsync();
            farm = await dbContext.Farms.Include(f=>f.Plots).OrderBy(f=>f.Id).FirstOrDefaultAsync(f=>f.TenantId==tenant.Id);
            if (farm is null)
            {
                farm = new();
                farm.Name = tenant.Name;
            }
            landPlot = farm.Plots?.FirstOrDefault() ?? new();
            StateHasChanged();
        }
        private async Task Next()
        {
            if(!await ProcessStepOnNextTransition()) return;
            Step++;
            StateHasChanged();
        }
        private void Prev()
        {
            Step--;
            StateHasChanged();
        }
        private async Task<bool> ProcessStepOnNextTransition()
        {
            switch (Step)
            {
                case 1:
                    if (farmEditor is not null && farmEditor.editContext.IsModified())
                    {
                        if (!farmEditor.editContext.Validate()) return false;
                        await farmEditor.OnSubmit();
                    }
                    break;
                case 2:
                    if (landPlotEditor is not null && landPlotEditor.editContext.IsModified())
                    {
                        if (!landPlotEditor.editContext.Validate()) return false;
                        await landPlotEditor.OnSubmit();
                    }
                    break;
            }
            return true;
        }

    }
}
