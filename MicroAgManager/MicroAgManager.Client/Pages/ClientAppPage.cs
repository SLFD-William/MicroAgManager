using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MicroAgManager.Client.Pages
{
    public class ClientAppPage:ComponentBase,IDisposable
    {
        [Inject] protected ClientApplicationStateProvider _app { get; set; }
        [Inject] private IJSRuntime _jsRuntime { get; set; }
        [Inject] private NavigationManager _nm { get; set; }

        public void Dispose()
        {
            _app.OnLocationChanged -= LocationChanged;
            _app.OnDataBaseInitialized -= DbInitialized;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !_app.Initialized)
            {
                await _app.FirstRenderInitialize(_jsRuntime, _nm);
                StateHasChanged();
            }
        }
        protected override void OnInitialized()
        {
            _app.OnLocationChanged += LocationChanged;
            _app.OnDataBaseInitialized += DbInitialized;
        }

        protected virtual void DbInitialized() => StateHasChanged();

        protected void LocationChanged(string obj) => StateHasChanged();
        
    }
}
