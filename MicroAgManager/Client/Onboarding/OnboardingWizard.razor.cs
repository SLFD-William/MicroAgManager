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
        private const string Step2 = "Farm Plots";
        private const string Step3 = "Livestock";
        private const string Step4 = "Complete";


        [Inject] FrontEndAuthenticationStateProvider authentication { get; set; }
        [CascadingParameter] DataSynchronizer dbSync { get; set; }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        protected TenantModel tenant;


        private FarmEditor? farmEditor;
        private LandPlotList? landPlotList;
        protected FarmLocationModel farm = new();

        private LandPlotEditor? landPlotEditor;
        protected LandPlotModel landPlot = new();
        private LivestockTypeModel? livestockType;

        private Wizard? wizard;
        private bool _buttonsVisible = true;
        private Task<bool> ShowButtons()
        {
            if (string.IsNullOrEmpty(wizard?.ActiveStep?.Name)) return Task.FromResult( _buttonsVisible);
            _buttonsVisible = true;
            if (wizard.ActiveStep.Name == Step3)
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
            //while (!dbContext.Tenants.Any(t => t.Id == authentication.TenantId()))
            //    await Task.Delay(100);


            tenant = await dbContext.Tenants.SingleAsync(t => t.Id == authentication.TenantId());
            farm = await dbContext.Farms.OrderBy(f => f.Id).FirstOrDefaultAsync(f => f.TenantId == tenant.Id)
                ?? new() { Name = tenant.Name };

            landPlot = dbContext.LandPlots.OrderBy(p=>p.ModifiedOn).FirstOrDefault(p=>p.FarmLocationId==farm.Id) ?? new();
            livestockType = await dbContext.LivestockTypes.FirstOrDefaultAsync();

            StateHasChanged();
        }
        private void OnboardingWizard_DatabaseUpdated()=>Task.Run(FreshenData);
        private async Task<bool> CanStepAdvance()
        {
            if (wizard?.ActiveStep?.Name == Step1)
                if (farmEditor is not null )
                {
                    if (farmEditor.editContext.IsModified() && !farmEditor.editContext.Validate()) return false;
                    if(farmEditor.editContext.IsModified()) await farmEditor.OnSubmit();
                }
            if (wizard?.ActiveStep?.Name == Step2)
                if (landPlotEditor is not null )
                {
                    if(landPlotEditor.editContext.IsModified() && !landPlotEditor.editContext.Validate()) return false;
                    if(landPlotEditor.editContext.IsModified()) await landPlotEditor.OnSubmit();
                }
            return true;
        }
        private async Task FarmLocationUpdated(FarmLocationModel args)
        {
            while (!dbContext.Farms.Any(t => t.Id == args.Id))
                await Task.Delay(100);
            
            await FreshenData();
        }
        

        private async Task LandPlotUpdated(LandPlotModel args) {
            if(args.Id>0)
            while (!dbContext.LandPlots.Any(t => t.Id == args.Id))
                await Task.Delay(100);

            await FreshenData();
            landPlotList?._listComponent.Update();
        }

        public ValueTask DisposeAsync()
        {
            dbSync.OnUpdate -= OnboardingWizard_DatabaseUpdated;
            return ValueTask.CompletedTask;
        }
    }
}
