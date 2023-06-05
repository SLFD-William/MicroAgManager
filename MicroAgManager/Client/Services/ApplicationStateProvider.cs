using BackEnd.Infrastructure;
using FrontEnd.Data;
using FrontEnd.Persistence;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace FrontEnd.Services
{
    public class ApplicationStateProvider : IAsyncDisposable
    {
        private FrontEndAuthenticationStateProvider _authentication;
        private NavigationManager _navigation;
        

        private static HubConnection? _hubConnection;
        private static FrontEndDbContext _dbContext;
        private static DataSynchronizer _dbSynchonizer;
        private static IFrontEndApiServices _api;
        public FrontEndDbContext? dbContext { get => _dbContext;}
        public DataSynchronizer? dbSynchonizer { get => _dbSynchonizer; }
        public IFrontEndApiServices? api { get => _api; }
        public ApplicationStateProvider(FrontEndAuthenticationStateProvider authentication, IFrontEndApiServices api,  IDbContextFactory<FrontEndDbContext> dbContextFactory, IJSRuntime js, NavigationManager navigation)
        {
            _api = api;
            _authentication = authentication;
            _navigation = navigation;
            Task.Run(async() => await ImportScripts(js));
            _dbSynchonizer = new DataSynchronizer(js, dbContextFactory, _api);
            _authentication.AuthenticationStateChanged += Authentication_AuthenticationStateChanged;

            Task.Run(InitializeAsync);
            Task.Run(async()=> await _dbSynchonizer.SynchronizeInBackground());
        }
        private async Task ImportScripts(IJSRuntime js)
        {
            await js.InvokeAsync<IJSObjectReference>("import", "./_content/FrontEnd/frontEndJsInterop.js");
            await js.InvokeAsync<IJSObjectReference>("import", "./_content/Blazor.Geolocation.WebAssembly/blazorators.geolocation.g.js");
        }
            private async Task InitializeAsync()
        {
            _dbContext = await _dbSynchonizer.GetPreparedDbContextAsync();
            await _authentication.RefreshToken();
        }
        private async Task HandleAuthenticationChange()
        {
            if (_authentication.User?.Identity?.IsAuthenticated == true)
            {
                await InitializeNotificationHub();
                await _dbSynchonizer.SynchronizeInBackground();
            }
            else
                await DisposeAsync();
        }
        private void Authentication_AuthenticationStateChanged(Task<AuthenticationState> task) => Task.Run(HandleAuthenticationChange);
        private async Task InitializeNotificationHub()
        {
            if (_hubConnection != null && _hubConnection.State != HubConnectionState.Disconnected) return;
            var address = _navigation.ToAbsoluteUri("/notificationhub");

            _hubConnection = new HubConnectionBuilder()
                //.WithUrl(address)
                .WithUrl(address,
                    options => options.AccessTokenProvider = async () => await _authentication.GetJWT())
                .WithAutomaticReconnect()
                .AddMessagePackProtocol()
                .Build();
            _hubConnection.Closed += _hubConnection_Closed;
            _hubConnection.Reconnected += _hubConnection_Reconnected;
            _hubConnection.Reconnecting += _hubConnection_Reconnecting;
            _hubConnection.On<EntitiesModifiedNotification>("ReceiveEntitiesModifiedMessage",
                async (notifications) =>
                {
                    await _dbSynchonizer.HandleModifiedEntities(_authentication?.UserId() ?? Guid.NewGuid(), notifications);
                });
            try
            {
                await _hubConnection.StartAsync();
                var foo = _hubConnection.State;
                await _hubConnection.InvokeAsync("JoinGroup", _authentication?.TenantId() ?? Guid.NewGuid());
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
        private Task _hubConnection_Reconnecting(Exception? arg)
        {
            throw new NotImplementedException();
        }
        private async Task _hubConnection_Reconnected(string? arg)
        {
            await _hubConnection.InvokeAsync("JoinGroup", _authentication?.TenantId() ?? Guid.NewGuid());
        }
        private Task _hubConnection_Closed(Exception? arg)
        {
            throw new NotImplementedException();
        }
        private async Task DisposeNotificationHub()
        {
            if (_hubConnection is null) return;
            await _hubConnection.InvokeAsync("LeaveGroup", _authentication?.TenantId() ?? Guid.NewGuid());
            _hubConnection.Closed -= _hubConnection_Closed;
            _hubConnection.Reconnected -= _hubConnection_Reconnected;
            _hubConnection.Reconnecting -= _hubConnection_Reconnecting;
            await _hubConnection.DisposeAsync();
        }
        public async ValueTask DisposeAsync()
        {
            await DisposeNotificationHub();
        }
    }
}
