
using Domain.Entity;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.Shared
{
    public partial class Wizard : ComponentBase
    {
        protected internal List<WizardStep> Steps = new List<WizardStep>();
        [Parameter] public Func<Task<bool>>? CanStepAdvance { get; set; }
        [Parameter] public Func<Task<bool>>? CanStepRetreat { get; set; }
        [Parameter] public Func<Task<bool>>? ShowAdvancementButtons { get; set; }
        [Parameter] public Func<Task<bool>>? ShowStepNavigation { get; set; }
        
        [Parameter] public string Id { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public WizardStep ActiveStep { get; set; }
        [Parameter] public int ActiveStepIx { get; set; }
        [Parameter] public bool IsNestedWizard { get; set; } = false;
        [Parameter] public bool IsLastStep { get; set; }
        [Parameter] public EventCallback<bool> Completed { get; set; }


        protected async internal Task GoBack()
        {
            var canStepRetreat = (CanStepRetreat is null) ? true : await CanStepRetreat.Invoke();
            if (!canStepRetreat) return;
            if (ActiveStepIx > 0)
                SetActive(Steps[ActiveStepIx - 1]);
            if (ActiveStepIx <= 0)
                await Completed.InvokeAsync(false);
        }

        /// <summary>
        /// Sets the <see cref="ActiveStep"/> to the next Index
        /// </summary>
        protected async internal Task GoNext()
        {
            var canStepAdvance = (CanStepAdvance is null) ? true : await CanStepAdvance.Invoke();
            if (!canStepAdvance) return;
            if (ActiveStepIx < Steps.Count - 1  )
                SetActive(Steps[Steps.IndexOf(ActiveStep) + 1]);
            if (ActiveStepIx >= Steps.Count - 1)
                await Completed.InvokeAsync(true);
        }

        protected internal void SetActive(WizardStep step)
        {
            ActiveStep = step ?? throw new ArgumentNullException(nameof(step));

            ActiveStepIx = StepsIndex(step);
            if (ActiveStepIx == Steps.Count - 1)
                IsLastStep = true;
            else
                IsLastStep = false;
        }

        /// <summary>
        /// Retrieves the index of the current <see cref="WizardStep"/> in the Step List
        /// </summary>
        /// <param name="step">The WizardStep</param>
        /// <returns></returns>
        public int StepsIndex(WizardStep step) => StepsIndexInternal(step);
        protected int StepsIndexInternal(WizardStep step)
        {
            if (step == null)
                throw new ArgumentNullException(nameof(step));

            return Steps.IndexOf(step);
        }
        /// <summary>
        /// Adds a <see cref="WizardStep"/> to the WizardSteps list
        /// </summary>
        /// <param name="step"></param>
        protected internal void AddStep(WizardStep step)
        {
            Steps.Add(step);
        }
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                SetActive(Steps[0]);
                StateHasChanged();
            }
        }
    }
}