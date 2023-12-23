using FrontEnd.Persistence;
using MicroAgManager.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;

namespace MicroAgManager.Services
{
    internal class ApplicationState:IDisposable
    {
        public static FrontEndDbContext _dbContext;
        public event Action? OnDbInitialized;
        public event Action? OnDbUpdate;

        private static List<long> _farmNameSelectedFilter = new();
        private static List<string> _usageSelectedFilter = new();
        private static AuthenticationStateProvider _authentication;
        private static DataSynchronizer _dbSynchonizer;
        private static NavigationManager _navigationManager;

        public ApplicationState(AuthenticationStateProvider authentication, DataSynchronizer dbSynchonizer, NavigationManager navigationManager, IConfiguration config)
        {
            _authentication = authentication;
            _authentication.AuthenticationStateChanged += Authentication_AuthenticationStateChanged;
            _navigationManager = navigationManager;
            _navigationManager.LocationChanged += NavigationManager_LocationChanged;
            _dbSynchonizer = dbSynchonizer;
            _dbSynchonizer.OnDbInitialized += _dbSynchonizer_OnDbInitialized;
            _dbSynchonizer.OnUpdate += _dbSynchonizer_OnUpdate;
            Task.Run(InitializeApp);
            Task.Run(RedirectLandingToHomeIfAuthenticated);
        }
        public static List<long> FarmNameSelectedFilter { get => _farmNameSelectedFilter; set { _farmNameSelectedFilter = value; } }
        public static List<string> UsageSelectedFilter { get => _usageSelectedFilter; set { _usageSelectedFilter = value; } }

        private void _dbSynchonizer_OnUpdate()=>OnDbUpdate?.Invoke();

        private void _dbSynchonizer_OnDbInitialized()=>OnDbInitialized?.Invoke();

        private async Task InitializeApp()
        {
            _dbContext = await _dbSynchonizer.InitializeDbContextAsync();
            await EnsureDbSynchronizing();

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
                    OnDbUpdate?.Invoke();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + e.StackTrace);
                }

            }
        }
        private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e) => Task.Run(RedirectLandingToHomeIfAuthenticated);
        private void Authentication_AuthenticationStateChanged(Task<AuthenticationState> task) =>Task.Run(EnsureDbSynchronizing);
        public void Dispose()
        {
            _authentication.AuthenticationStateChanged -= Authentication_AuthenticationStateChanged;
            _navigationManager.LocationChanged -= NavigationManager_LocationChanged;
            _dbSynchonizer.OnDbInitialized -= _dbSynchonizer_OnDbInitialized;
            _dbSynchonizer.OnUpdate -= _dbSynchonizer_OnUpdate;
        }
        //public static ILogger _log;
        public static string CorrectedParametersUri(Dictionary<string, string> NewParameters)
        {
            var uri = new Uri(_navigationManager.Uri);
            var queryParameters = System.Web.HttpUtility.ParseQueryString(uri.Query);
            foreach (var parameter in NewParameters)
            {
                queryParameters.Remove(parameter.Key);
                if (!string.IsNullOrWhiteSpace(parameter.Value))
                    queryParameters.Add(parameter.Key, parameter.Value);
            }

            var baseUri = uri.GetLeftPart(UriPartial.Path);
            return queryParameters.Count == 0 ? baseUri : $"{baseUri}?{queryParameters}";
        }
        private async Task RedirectLandingToHomeIfAuthenticated()
        {
            if (_navigationManager.BaseUri == _navigationManager.Uri && await UserIsAuthenticated())
                _navigationManager.NavigateTo("/Home");

        }

    }
}
