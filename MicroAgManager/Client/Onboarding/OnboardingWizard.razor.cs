using Domain.Constants;
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
        private const string Step2 = "Farm Plots";
        private const string Step3 = "Livestock";
        private const string Step4 = "Complete";


        [Inject] FrontEndAuthenticationStateProvider authentication { get; set; }
        [Inject] ApplicationStateProvider app { get; set; }
        [Inject] NavigationManager navigationManager { get; set; }
        protected TenantModel tenant;


        private FarmEditor? farmEditor;
        private LandPlotList? landPlotList;
        protected FarmLocationModel farm = new();
        private LandPlotEditor? landPlotEditor;

        private long? landPlotId { get; set; }

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
        protected async override Task OnInitializedAsync()
        {
            app.dbSynchonizer.OnUpdate += OnboardingWizard_DatabaseUpdated;
            await FreshenData();

        }
        private void LandPlotSelected(LandPlotModel? plot)
        {
            landPlotId = plot?.Id;
            StateHasChanged();
            if (landPlotEditor is not null) Task.Run(landPlotEditor.FreshenData);
        }
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
            if (wizard?.ActiveStep?.Name == Step2)
                return await HandleLandPlotSubmission();
            return true;
        }
        private async Task<bool> HandleLandPlotSubmission()
        {
            if (landPlotEditor is not null)
            {
                if (landPlotEditor.editContext.IsModified() && !landPlotEditor.editContext.Validate()) return false;
                if (landPlotEditor.editContext.IsModified())
                {
                    await landPlotEditor.OnSubmit();
                    LandPlotSelected(null);
                }
                return true;
            }
            return false;
        }
        private async Task<bool> CanStepAdvance()
        {
            if (wizard?.ActiveStep?.Name == Step1)
                if (farmEditor is not null )
                {
                    if (farmEditor.editContext.IsModified() && !farmEditor.editContext.Validate()) return false;
                    if(farmEditor.editContext.IsModified()) await farmEditor.OnSubmit();
                    farm= (FarmLocationModel)farmEditor.editContext.Model;
                }
            if (wizard?.ActiveStep?.Name == Step2)
            {
                var advance= await HandleLandPlotSubmission();
                if (advance && (landPlotList?.Items?.Any(l => l.Usage == LandPlotUseConstants.Livestock) ?? false))
                {
                    InsertStepAfterActive();
                    return false;
                }
            }
            if (wizard?.ActiveStep?.Name == Step4) //Complete
                navigationManager.NavigateTo("",true);
         
            return true;
        }
        private void InsertStepAfterActive()
        {
            var currentIndex = wizard?.Steps.IndexOf(wizard.ActiveStep) ?? 0;
            var step = wizard?.Steps.FirstOrDefault(s => s.Name == Step3);
            if (step is not null)
            {
                wizard?.Steps.Remove(step);
                wizard?.Steps.Insert(currentIndex + 1, step);
                wizard?.SetActive(wizard.Steps[currentIndex + 1]);
            }
        }
        private async Task FarmLocationUpdated(FarmLocationModel args)
        {
            while (!app.dbContext.Farms.Any(t => t.Id == args.Id))
                await Task.Delay(100);
            
            await FreshenData();
        }
        private async Task LandPlotUpdated(LandPlotModel args)
        {
            if (args.Id > 0)
                while (!app.dbContext.LandPlots.Any(t => t.Id == args.Id))
                    await Task.Delay(100);

            LandPlotSelected(args);
            landPlotList?._listComponent.Update();
        }
        public ValueTask DisposeAsync()
        {
            app.dbSynchonizer.OnUpdate -= OnboardingWizard_DatabaseUpdated;
            return ValueTask.CompletedTask;
        }
    }
}
