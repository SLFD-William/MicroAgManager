using MicroAgManager.Data;
using MicroAgManager.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MicroAgManager.Components.Layout
{
    public partial class MainLayout: LayoutComponentBase,IDisposable
    {
        private static ApplicationState _appState;
        [Inject] DataSynchronizer dbSynchonizer { get; set; }
        [Inject] IConfiguration config { get; set; }
        [Inject] AuthenticationStateProvider authentication { get; set; }
        [Inject] NavigationManager navigationManager { get; set; }
        protected override void OnInitialized()
        {
            _appState = new ApplicationState(authentication, dbSynchonizer, navigationManager, config);
            _appState.OnDbInitialized += _appState_OnDbInitialized;
            _appState.OnDbUpdate += _appState_OnDbUpdate;
        }

        private void _appState_OnDbUpdate() => StateHasChanged();

        private void _appState_OnDbInitialized() => StateHasChanged();
        public void Dispose()
        {
            _appState.OnDbInitialized -= _appState_OnDbInitialized;
            _appState.OnDbUpdate -= _appState_OnDbUpdate;
        }
    }
}
