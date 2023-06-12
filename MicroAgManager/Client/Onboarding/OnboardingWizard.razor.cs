using Domain.Models;
using FrontEnd.Components.Farm;
using FrontEnd.Components.LandPlot;
using FrontEnd.Components.Shared;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Onboarding
{
    public partial class OnboardingWizard : IAsyncDisposable
    {
        private const string Step1 = "Farm Information";
        //private const string Step2 = "Farm Plots";
        //private const string Step3 = "Production";
        private const string Step2 = "Complete";


        [Inject] FrontEndAuthenticationStateProvider authentication { get; set; }
        [Inject] ApplicationStateProvider app { get; set; }
        [Inject] NavigationManager navigationManager { get; set; }
        protected TenantModel tenant;


        private FarmEditor? farmEditor;
        private LandPlotList? landPlotList;
        protected FarmLocationModel farm = new();

        private LandPlotEditor? landPlotEditor;
        //private long? landPlotId { get; set; }
        //private bool landPlotLooping { get; set; }=false;
        //private string? landPlotUsage { get; set; }


        //private LivestockTypeModel? livestockType;

        private Wizard? wizard;
        private bool _buttonsVisible = true;
        private Task<bool> ShowButtons()
        {
            if (string.IsNullOrEmpty(wizard?.ActiveStep?.Name)) return Task.FromResult( _buttonsVisible);
            _buttonsVisible = true;
            //if (wizard.ActiveStep.Name == Step3)
            //    _buttonsVisible = false; 
            return Task.FromResult(_buttonsVisible);
        }
        //private void LivestockTypeWizardCompleted(bool args)
        //{
        //    if (args)
        //        wizard?.GoNext();
        //    else
        //        wizard?.GoBack();
        //}
        protected async override Task OnInitializedAsync()
        {
            app.dbSynchonizer.OnUpdate += OnboardingWizard_DatabaseUpdated;
            await FreshenData();

        }
        //private void LandPlotSelected(LandPlotModel? plot)
        //{
        //    landPlotLooping = plot is null;
        //    landPlotId = plot?.Id;
        //    landPlotUsage = plot?.Usage;
        //    StateHasChanged();
        //    if (landPlotEditor is not null) Task.Run(landPlotEditor.FreshenData);
        //}
        private async Task FreshenData()
        {
            while (authentication.TenantId() == Guid.Empty)
                await Task.Delay(100);
            while (app.dbContext is null || !app.dbContext.Tenants.Any(t => t.GuidId == authentication.TenantId()))
                await Task.Delay(100);

            tenant = await app.dbContext.Tenants.SingleAsync(t => t.GuidId == authentication.TenantId());
            farm = await app.dbContext.Farms.OrderBy(f => f.Id).FirstOrDefaultAsync(f => f.TenantId == tenant.GuidId)
                ?? new() { Name = tenant.Name };

            //livestockType = await app.dbContext.LivestockTypes.OrderBy(t=>t.Id).FirstOrDefaultAsync();

            StateHasChanged();
        }
        private void OnboardingWizard_DatabaseUpdated()=>Task.Run(FreshenData);
        private async Task<bool> CanStepRepeat()
        {
            //if (wizard?.ActiveStep?.Name == Step2)
            //    if (landPlotEditor is not null)
            //    {
            //        if (landPlotEditor.editContext.IsModified() && !landPlotEditor.editContext.Validate()) return false;
            //        if (landPlotEditor.editContext.IsModified()) await landPlotEditor.OnSubmit();
            //        LandPlotSelected(null);
            //    }
            return true;
        }
        private async Task<bool> CanStepAdvance()
        {
            if (wizard?.ActiveStep?.Name == Step1)
                if (farmEditor is not null )
                {
                    if (farmEditor.editContext.IsModified() && !farmEditor.editContext.Validate()) return false;
                    if(farmEditor.editContext.IsModified()) await farmEditor.OnSubmit();
                }
            if (wizard?.ActiveStep?.Name == Step2) //Complete
            {
                navigationManager.NavigateTo("");
            }
            //if (wizard?.ActiveStep?.Name == Step2)
            //    if (landPlotEditor is not null )
            //    {
            //        if(landPlotEditor.editContext.IsModified() && !landPlotEditor.editContext.Validate()) return false;
            //        if(landPlotEditor.editContext.IsModified()) await landPlotEditor.OnSubmit();
            //        LandPlotSelected((LandPlotModel)landPlotEditor.editContext.Model);
            //    }
            return true;
        }
        private async Task FarmLocationUpdated(FarmLocationModel args)
        {
            while (!app.dbContext.Farms.Any(t => t.Id == args.Id))
                await Task.Delay(100);
            
            await FreshenData();
        }
        

        //private async Task LandPlotUpdated(LandPlotModel args) {
        //    if(args.Id>0)
        //    while (!app.dbContext.LandPlots.Any(t => t.Id == args.Id))
        //        await Task.Delay(100);

        //    LandPlotSelected(args);
        //    landPlotList?._listComponent.Update();
        //}

        public ValueTask DisposeAsync()
        {
            app.dbSynchonizer.OnUpdate -= OnboardingWizard_DatabaseUpdated;
            return ValueTask.CompletedTask;
        }
    }
}
