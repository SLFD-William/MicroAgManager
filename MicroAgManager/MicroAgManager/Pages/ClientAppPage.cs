using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MicroAgManager.Pages
{
    public class ClientAppPage : ComponentBase, IDisposable
    {
        [Inject] private IJSRuntime _jsRuntime { get; set; }
        [Inject] private NavigationManager _nm { get; set; }

        public void Dispose()
        {

        }
    }
}
