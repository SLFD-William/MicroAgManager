using Domain.Models;
using FrontEnd.Components.LivestockBreed;
using FrontEnd.Components.LivestockStatus;
using FrontEnd.Components.Shared;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockType
{
    public partial class AddLivestockTypeWizard
    {
        private const string Step1 = "Type Definition";
        private const string Step2 = "Breeds";
        private const string Step3 = "Status Definition";
        private const string Step4 = "Feeding";
        

        private Wizard? wizard;
        [Inject] ApplicationStateProvider app { get; set; }
        [CascadingParameter] public LivestockTypeModel livestockType { get; set; }
        [Parameter] public EventCallback<bool> Completed { get; set; }
        [Parameter] public bool IsNestedWizard { get; set; } = false;


        private LivestockTypeEditor? livestockTypeEditor;
        private LivestockBreedEditor? livestockBreedEditor;
        private LivestockBreedList? livestockBreedList;
        private LivestockStatusEditor? livestockStatusEditor;

        private void LivestockFeedWizardCompleted(bool args)
        {
            if (args)
                wizard?.GoNext();
            else
                wizard?.GoBack();
        }

        private bool _buttonsVisible = true;
        private Task<bool> ShowButtons()
        {
            if (string.IsNullOrEmpty(wizard?.ActiveStep?.Name)) return Task.FromResult( _buttonsVisible);
            _buttonsVisible = true;
            if (wizard.ActiveStep.Name == Step3)
                _buttonsVisible = false;
            return Task.FromResult(_buttonsVisible);
        }
        protected async override Task OnInitializedAsync()
        {
            if (livestockType is null) livestockType = new();
            await PopulateRelations();
            
        }
        private async Task PopulateRelations()
        {
            if (livestockType.Id > 0)
            {
                livestockType.Breeds = await app.dbContext.LivestockBreeds.Where(b => b.LivestockTypeId == livestockType.Id).ToListAsync();
            }
            StateHasChanged();
        }
        private async Task<bool> CanStepAdvance()
        {
            if (wizard?.ActiveStep?.Name == Step1)
                if (livestockTypeEditor is not null && livestockTypeEditor.editContext.IsModified())
                {
                    if (!livestockTypeEditor.editContext.Validate()) return false;
                    await livestockTypeEditor.OnSubmit();
                }
            if (wizard?.ActiveStep?.Name == Step2)
                if (livestockBreedEditor is not null && livestockBreedEditor.editContext.IsModified())
                {
                    if (!livestockBreedEditor.editContext.Validate()) return false;
                    await livestockBreedEditor.OnSubmit();
                }
            if (wizard?.ActiveStep?.Name == Step3)
                if (livestockStatusEditor is not null && livestockStatusEditor.editContext.IsModified())
                {
                    if (!livestockStatusEditor.editContext.Validate()) return false;
                    await livestockStatusEditor.OnSubmit();
                }
            return true;
        }
        private async Task<bool> CanStepRetreat()
        {
            if (wizard?.ActiveStep?.Name == Step1 && IsNestedWizard)
                await Completed.InvokeAsync(false);
               
            return true;
        }

        private async Task LivestockBreedUpdated(LivestockBreedModel args)
        {
            if (args.Id > 0)
                while (!app.dbContext.LivestockBreeds.Any(t => t.Id == args.Id))
                    await Task.Delay(100);

            await PopulateRelations();
            livestockBreedList?._listComponent.Update();
        }

        private void LivestockTypeUpdated(LivestockTypeModel args) => livestockType = args;
        private void LivestockStatusUpdated(LivestockStatusModel args) => livestockType.Statuses.Add(args);
    }
}
