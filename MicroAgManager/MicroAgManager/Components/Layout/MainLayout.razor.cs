using FrontEnd.Persistence;
using MicroAgManager.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;

namespace MicroAgManager.Components.Layout
{
    public partial class MainLayout: LayoutComponentBase,IDisposable
    {
        private static ILogger _log;
        private static FrontEndDbContext _dbContext;
        private static AuthenticationStateProvider _authentication;
        private static DataSynchronizer _dbSynchonizer;
        private static NavigationManager _navigationManager;

        [Inject] DataSynchronizer dbSynchonizer { get; set; }
        [Inject] IConfiguration config { get; set; }
        [Inject] AuthenticationStateProvider authentication { get; set; }
        [Inject] NavigationManager navigationManager { get; set; }
        
        
        protected async override Task OnInitializedAsync()
        {
            _authentication = authentication;
            _authentication.AuthenticationStateChanged += Authentication_AuthenticationStateChanged;
            _navigationManager = navigationManager;
            _navigationManager.LocationChanged += NavigationManager_LocationChanged;
            if (_dbContext == null || _dbSynchonizer == null)
            {
                _dbSynchonizer = dbSynchonizer;
                _dbContext = await _dbSynchonizer.InitializeDbContextAsync();
                await EnsureDbSynchronizing();
                //_log = new DatabaseLoggingProvider(_dbContext, config).CreateLogger("ClientLogging");
            }
            await RedirectLandingToHomeIfAuthenticated();
        }
        public void Dispose()
        {
            _authentication.AuthenticationStateChanged -= Authentication_AuthenticationStateChanged;
            _navigationManager.LocationChanged -= NavigationManager_LocationChanged;
        }
        private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e) => InvokeAsync(RedirectLandingToHomeIfAuthenticated);
        private void Authentication_AuthenticationStateChanged(Task<AuthenticationState> task) => InvokeAsync(EnsureDbSynchronizing);


        private async Task RedirectLandingToHomeIfAuthenticated()
        {
            if (_navigationManager.BaseUri == _navigationManager.Uri && await UserIsAuthenticated())
                _navigationManager.NavigateTo("/Home");

        }
        private async Task<bool> UserIsAuthenticated()
        {
            var user = await _authentication.GetAuthenticationStateAsync();
            return user.User?.Identity?.IsAuthenticated == true;
        }
        private async Task EnsureDbSynchronizing()
        {
            if (await UserIsAuthenticated())
            {
                try
                {
                    //Console.WriteLine("Initializing SignalR");
                    //await InitializeNotificationHub();
                    await _dbSynchonizer.EnsureSynchronizingAsync(null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + e.StackTrace);
                }

            }
        }
    }
}
