using BackEnd.Infrastructure;
using FrontEnd.Persistence;
using MicroAgManager.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.SignalR.Client;
using System.Security.Claims;

namespace MicroAgManager.Services
{
    public class ApplicationState:ComponentBase,IDisposable
    {
        public static FrontEndDbContext _dbContext;
        public event Action? OnDbInitialized;
        public event Action? OnDbUpdate;
        public event Action? OnLocationChange;

        private static AuthenticationStateProvider _authentication;
        private static DataSynchronizer _dbSynchonizer;
        private static NavigationManager _navigationManager;
        private static HubConnection? _hubConnection;
        private static IConfiguration? _config;
        private static HashSet<ModifiedEntityPushNotification> _entityPush = new HashSet<ModifiedEntityPushNotification>();


        public FrontEndDbContext DbContext { get => _dbContext; }
        public ApplicationState(AuthenticationStateProvider authentication, DataSynchronizer dbSynchonizer, NavigationManager navigationManager, IConfiguration config)
        {
            _config = config;
            _authentication = authentication;
            _authentication.AuthenticationStateChanged += Authentication_AuthenticationStateChanged;
            _navigationManager = navigationManager;
            _navigationManager.LocationChanged += NavigationManager_LocationChanged;
            _dbSynchonizer = dbSynchonizer;
            _dbSynchonizer.OnDbInitialized += _dbSynchonizer_OnDbInitialized;
            _dbSynchonizer.OnUpdate += _dbSynchonizer_OnUpdate;
            Task.Run(InitializeApp);
            Task.Run(StartPushHandlerTimer);
            Task.Run(RedirectLandingToHomeIfAuthenticated);
        }
        #region Tree Nodes
        private static List<string> _selectedTreeNodes = new();
        private static List<string> _expandedTreeNodes = new();
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
        #endregion
        #region Selected Filters
        private static List<long> _farmSelectedFilter = new();
        private static List<long> _animalSelectedFilter = new();
        private static List<long> _locationSelectedFilter = new();
        private static List<long> _breedSelectedFilter = new();
        private static List<string> _usageSelectedFilter = new();
        public static List<long> AnimalSelectedFilter { get => _animalSelectedFilter; set { _animalSelectedFilter = value; } }
        public static List<long> BreedSelectedFilter { get => _breedSelectedFilter; set { _breedSelectedFilter = value; } }
        public static List<long> LocationSelectedFilter { get => _locationSelectedFilter; set { _locationSelectedFilter = value; } }
        public static List<long> FarmSelectedFilter { get => _farmSelectedFilter; set { _farmSelectedFilter = value; } }
        public static List<string> UsageSelectedFilter { get => _usageSelectedFilter; set { _usageSelectedFilter = value; } }

        #endregion
        public static bool CanAddFarm() => _dbContext.Farms.Count() < 1;
        private void _dbSynchonizer_OnUpdate()=>OnDbUpdate?.Invoke();
        private void _dbSynchonizer_OnDbInitialized()=>OnDbInitialized?.Invoke();
        public Guid? TenantId { get; private set; }
        public Guid? UserId { get; private set; }
        private async Task InitializeApp()
        {
            _dbContext = await _dbSynchonizer.InitializeDbContextAsync();
            await SetupAuthenticatedServices();
        }
        private async Task StartPushHandlerTimer()
        {
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
            while (await timer.WaitForNextTickAsync())
                while (_entityPush.Any())
                { 
                    var entities=_entityPush.OrderBy(x=>x.ServerModifiedTime).ToList();
                    _entityPush.Clear();
                    foreach (var entity in entities)
                        await _dbSynchonizer.HandleEntityPushNotification(entity);
                }
        }
        private async Task<bool> UserIsAuthenticated()
        {
            var user = await _authentication.GetAuthenticationStateAsync();
            var isAuthenticated= user.User?.Identity?.IsAuthenticated == true;

            TenantId = isAuthenticated ? Guid.Parse(user.User?.Claims.FirstOrDefault(c => c.Type == "TenantId")?.Value) : null;
            UserId = isAuthenticated ? Guid.Parse(user.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value) : null;

            return isAuthenticated;
        }
        private async Task EnsureDbSynchronizing()
        {
            if (TenantId.HasValue && TenantId.Value!=Guid.Empty)
            {
                try
                {
                    await _dbSynchonizer.EnsureSynchronizingAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + e.StackTrace);
                }
            }
        }

        private async Task InitializeNotificationHub()
        {
            Console.WriteLine("Entering Hub Initialization");
            if (_hubConnection != null && _hubConnection.State != HubConnectionState.Disconnected) return;
            var address = new Uri($"{_config["BackendUrl"]}/notificationhub"); //_navigationManager.ToAbsoluteUri("/notificationhub");
            Console.WriteLine($"Hub Address {address.AbsoluteUri}");

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(address)
                .WithAutomaticReconnect()
                .AddMessagePackProtocol()
                .Build();
            _hubConnection.Closed += _hubConnection_Closed;
            _hubConnection.Reconnected += _hubConnection_Reconnected;
            _hubConnection.Reconnecting += _hubConnection_Reconnecting;
            _hubConnection.On<ModifiedEntityPushNotification>("ReceiveModifiedEntityPush",(notification)=> _entityPush.Add(notification));
            try
            {
                Console.WriteLine("Starting Signalr Listener");
                await _hubConnection.StartAsync();
                var foo = _hubConnection.State;
                Console.WriteLine($"Hub Listener State {foo}");
                await _hubConnection.InvokeAsync("JoinGroup", TenantId ?? Guid.NewGuid());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex);
                throw;
            }
        }
        

        
        private Task _hubConnection_Reconnecting(Exception? arg)
        {
            throw new NotImplementedException();
        }
        private async Task _hubConnection_Reconnected(string? arg)
        {
            await _hubConnection.InvokeAsync("JoinGroup", TenantId ?? Guid.NewGuid());
        }
        private Task _hubConnection_Closed(Exception? arg)
        {
            throw new NotImplementedException();
        }
        private async Task DisposeNotificationHub()
        {
            if (_hubConnection is null) return;
            await _hubConnection.InvokeAsync("LeaveGroup", TenantId ?? Guid.NewGuid());
            _hubConnection.Closed -= _hubConnection_Closed;
            _hubConnection.Reconnected -= _hubConnection_Reconnected;
            _hubConnection.Reconnecting -= _hubConnection_Reconnecting;
            await _hubConnection.DisposeAsync();
        }
        private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e) => Task.Run(RedirectLandingToHomeIfAuthenticated);
        private void Authentication_AuthenticationStateChanged(Task<AuthenticationState> task) => Task.Run(SetupAuthenticatedServices);
        private async Task SetupAuthenticatedServices()
        {
            var authenticated = await UserIsAuthenticated();
            if (!authenticated)
            {
                await DisposeNotificationHub();
                return;
            }
            await InitializeNotificationHub();
            await EnsureDbSynchronizing();
            
        }

        public void Dispose()
        {
            _authentication.AuthenticationStateChanged -= Authentication_AuthenticationStateChanged;
            _navigationManager.LocationChanged -= NavigationManager_LocationChanged;
            _dbSynchonizer.OnDbInitialized -= _dbSynchonizer_OnDbInitialized;
            _dbSynchonizer.OnUpdate -= _dbSynchonizer_OnUpdate;
            Task.Run(DisposeNotificationHub);
        }
        //public static ILogger _log;

        public static void NavigateTo(Dictionary<string, string> NewParameters)=>_navigationManager.NavigateTo(CorrectedParametersUri(NewParameters));

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

            OnLocationChange?.Invoke();
        }

    }
}
