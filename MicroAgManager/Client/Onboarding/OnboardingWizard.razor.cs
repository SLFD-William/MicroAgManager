using FrontEnd.Data;
using FrontEnd.Persistence;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Onboarding
{
    public partial class OnboardingWizard
    {
        [CascadingParameter] DataSynchronizer dbSync { get; set; }
        public int Step { get; set; } = 1;
        private FrontEndDbContext dbContext { get; set; }
        protected async override Task OnInitializedAsync()
        {
            if (dbSync is null) return;
            dbContext = await dbSync.GetPreparedDbContextAsync();
            StateHasChanged();
        }
        private void Next()
        {
            Step++;
            StateHasChanged();
        }
        private void Prev()
        {
            Step--;
            StateHasChanged();
        }
    }
}
