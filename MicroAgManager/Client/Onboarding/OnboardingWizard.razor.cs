using Domain.Constants;
using Domain.Models;
using FrontEnd.Components.Farm;
using FrontEnd.Components.LandPlot;
using FrontEnd.Components.Shared;
using FrontEnd.Data;
using FrontEnd.Persistence;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Onboarding
{
    public partial class OnboardingWizard : IAsyncDisposable
    {
        private const string Step1 = "Farm Information";
        private const string Step2 = "Farm Location";
        private const string Step3 = "Main Plot";
        private const string Step4 = "Complete";


        [Inject] FrontEndAuthenticationStateProvider authentication { get; set; }
        [CascadingParameter] DataSynchronizer dbSync { get; set; }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        protected TenantModel tenant;

        private FarmEditor? farmEditor;
        protected FarmLocationModel farm = new();

        private LandPlotEditor? landPlotEditor;
        protected LandPlotModel landPlot = new();
        private LivestockTypeModel? livestockType;
        private string farmProduction = string.Empty;

        private Wizard? wizard;
        private bool _buttonsVisible = true;
        private Task<bool> ShowButtons()
        {
            if (string.IsNullOrEmpty(wizard?.ActiveStep?.Name)) return Task.FromResult( _buttonsVisible);
            _buttonsVisible = true;
            if ((farmProduction == LandPlotUseConstants.Livestock || landPlot.Usage == LandPlotUseConstants.Livestock) && wizard.ActiveStep.Name == Step3)
                _buttonsVisible = false; ;
            return Task.FromResult(_buttonsVisible);
        }
        private void LivestockTypeWizardCompleted(bool args)
        {
            if (args)
                wizard?.GoNext();
            else
                wizard?.GoBack();
        }
        protected async override Task OnInitializedAsync()
        {
            dbSync.OnUpdate += OnboardingWizard_DatabaseUpdated;
            await FreshenData();
       
        }
        private async Task FreshenData()
        {
           if(dbContext is null) dbContext = await dbSync.GetPreparedDbContextAsync();
            while (authentication.TenantId() == Guid.Empty)
                await Task.Delay(100);
            while (!dbContext.Tenants.Any(t => t.Id == authentication.TenantId()))
                await Task.Delay(100);


            tenant = await dbContext.Tenants.SingleAsync(t => t.Id == authentication.TenantId());
            farm = await dbContext.Farms.Include(f => f.Plots).OrderBy(f => f.Id).FirstOrDefaultAsync(f => f.TenantId == tenant.Id)
                ?? new() { Name = tenant.Name };

            landPlot = farm.Plots?.FirstOrDefault() ?? new();
            livestockType = await dbContext.LivestockTypes.FirstOrDefaultAsync();

            StateHasChanged();
        }
        private void OnboardingWizard_DatabaseUpdated()=>Task.Run(FreshenData);
        private async Task<bool> CanStepAdvance()
        {
            if (wizard?.ActiveStep?.Name == Step1)
                if (farmEditor is not null && farmEditor.editContext.IsModified())
                {
                    if (!farmEditor.editContext.Validate()) return false;
                    await farmEditor.OnSubmit();
                }
            if (wizard?.ActiveStep?.Name == Step2)
                if (landPlotEditor is not null && landPlotEditor.editContext.IsModified())
                {
                    if (!landPlotEditor.editContext.Validate()) return false;
                    await landPlotEditor.OnSubmit();
                }
            return true;
        }
        private void FarmLocationUpdated(FarmLocationModel args) => farm = args;
        private void LandPlotUpdated(LandPlotModel args) => landPlot = args;

        public ValueTask DisposeAsync()
        {
            dbSync.OnUpdate -= OnboardingWizard_DatabaseUpdated;
            return ValueTask.CompletedTask;
        }
    }
}
