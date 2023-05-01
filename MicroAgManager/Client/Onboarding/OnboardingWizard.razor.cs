using Domain.Models;
using FrontEnd.Persistence;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using FrontEnd.Components.Farm;
using FrontEnd.Components.LandPlot;
using FrontEnd.Components.LivestockType;
using FrontEnd.Services;
using Domain.Constants;

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
            
        }
        [Inject] FrontEndAuthenticationStateProvider authentication { get; set; }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        public int Step { get; set; } = 1;
        
        protected TenantModel tenant;

        private FarmEditor? farmEditor;
        protected FarmLocationModel farm=new();
        
        private LandPlotEditor? landPlotEditor;
        protected LandPlotModel landPlot = new();
        private LivestockTypeModel livestockType;
        private string farmProduction = string.Empty;
        private void FarmLocationUpdated(FarmLocationModel args)=>farm = args;
        private void LandPlotUpdated(LandPlotModel args) => landPlot = args;
        private async Task LivestockTypeWizardCompleted(bool args)
        {
            if (args)
                Step = await ProcessStepOnNextTransition();
            else
                Step = ProcessStepOnPrevTransition();
        }
        
        protected async override Task OnInitializedAsync()
        {
            if (dbContext is null) return;
            tenant = await dbContext.Tenants.SingleOrDefaultAsync(t=>t.Id==authentication.TenantId());
            farm = await dbContext.Farms.Include(f=>f.Plots).OrderBy(f=>f.Id).FirstOrDefaultAsync(f=>f.TenantId==tenant.Id);
            if (farm is null)
            {
                farm = new();
                farm.Name = tenant.Name;
            }
            landPlot = farm.Plots?.FirstOrDefault() ?? new();
            livestockType= await dbContext.LivestockTypes.FirstOrDefaultAsync();

            StateHasChanged();
        }
        private async Task Next()
        {
            Step = await ProcessStepOnNextTransition();
            StateHasChanged();
        }
        private bool ShowPrevious()
        {
            var show = Step != 1;
            if(Step == (int)Steps.FarmProduction && (farmProduction == LandPlotUseConstants.Livestock || landPlot.Usage == LandPlotUseConstants.Livestock))
                show = false;

            return show; }
        private bool ShowNext()
        {
            var show = Step != 4;
            if (Step == (int)Steps.FarmProduction && (farmProduction == LandPlotUseConstants.Livestock || landPlot.Usage == LandPlotUseConstants.Livestock))
                show = false;
            return show; }
        private void Prev()
        {
            Step = ProcessStepOnPrevTransition();
            StateHasChanged();
        }
        private int ProcessStepOnPrevTransition()
        {
            return Step-1;
        }
        private async Task<int> ProcessStepOnNextTransition()
        {
            switch (Step)
            {
                case (int)Steps.FarmLocation:
                    if (farmEditor is not null && farmEditor.editContext.IsModified())
                    {
                        if (!farmEditor.editContext.Validate()) return (int)Steps.FarmLocation;
                        await farmEditor.OnSubmit();
                    }
                    return (int)Steps.MainPlot;
                case (int)Steps.MainPlot:
                    if (landPlotEditor is not null && landPlotEditor.editContext.IsModified())
                    {
                        if (!landPlotEditor.editContext.Validate()) return (int)Steps.MainPlot;
                        await landPlotEditor.OnSubmit();
                    }
                    return (int)Steps.FarmProduction;
            }
            return Step+1;
        }

    }
}
