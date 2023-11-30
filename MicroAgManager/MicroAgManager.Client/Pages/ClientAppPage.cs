using Domain.Context;
using MicroAgManager.Client.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace MicroAgManager.Client.Pages
{
    public class ClientAppPage:ComponentBase
    {
        [Inject] protected ClientApplicationStateProvider _app { get; set; }
        [Inject] private IJSRuntime _jsRuntime { get; set; }
        [Inject] private NavigationManager _nm { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !_app.Initialized)
            {
                await _app.FirstRenderInitialize(_jsRuntime, _nm);
                StateHasChanged();
            }
        }
    }
}
