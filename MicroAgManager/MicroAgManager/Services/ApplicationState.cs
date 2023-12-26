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

        private static List<long> _farmSelectedFilter = new();
        private static List<long> _animalSelectedFilter = new();
        private static List<long> _locationSelectedFilter = new();
        private static List<long> _breedSelectedFilter = new();
        private static List<string> _usageSelectedFilter = new();
        private static List<string> _selectedTreeNodes = new();
        private static List<string> _expandedTreeNodes = new();
        private static AuthenticationStateProvider _authentication;
        private static DataSynchronizer _dbSynchonizer;
        private static NavigationManager _navigationManager;

        public FrontEndDbContext DbContext { get => _dbContext; }
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
        public static void UpdageSelectedNodeState(string selectedNode, bool selected)
        {
            if (selected)
            {
                if (!SelectedTreeNodes.Contains(selectedNode)) SelectedTreeNodes.Add(selectedNode);
            }
            else
            {
                if (SelectedTreeNodes.Contains(selectedNode)) SelectedTreeNodes.Remove(selectedNode);
            }
        }
        public static void UpdageExpandedNodeState(string expandedNode, bool expanded)
        {
            if (expanded)
            {
                if (!ExpandedTreeNodes.Contains(expandedNode)) ExpandedTreeNodes.Add(expandedNode);
            }
            else
            {
                if (ExpandedTreeNodes.Contains(expandedNode)) ExpandedTreeNodes.Remove(expandedNode);
            }
        }
        public static List<string> SelectedTreeNodes { get => _selectedTreeNodes; private set { _selectedTreeNodes = value; } }
        public static List<string> ExpandedTreeNodes { get => _expandedTreeNodes; private set { _expandedTreeNodes = value; } }

        public static List<long> AnimalSelectedFilter { get => _animalSelectedFilter; set { _animalSelectedFilter = value; } }
        public static List<long> BreedSelectedFilter { get => _breedSelectedFilter; set { _breedSelectedFilter = value; } }
        public static List<long> LocationSelectedFilter { get => _locationSelectedFilter; set { _locationSelectedFilter = value; } }
        public static List<long> FarmSelectedFilter { get => _farmSelectedFilter; set { _farmSelectedFilter = value; } }
        public static List<string> UsageSelectedFilter { get => _usageSelectedFilter; set { _usageSelectedFilter = value; } }

        public static bool CanAddFarm() => _dbContext.Farms.Count() < 1;
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
        public static bool FieldIsInQueryString(string field) => _navigationManager.Uri.Contains(field);
        
        private async Task RedirectLandingToHomeIfAuthenticated()
        {
            if (_navigationManager.BaseUri == _navigationManager.Uri && await UserIsAuthenticated())
                _navigationManager.NavigateTo("/Home");

        }

    }
}
