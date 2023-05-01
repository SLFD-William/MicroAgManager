using BackEnd.Models;
using FrontEnd.Data;
using FrontEnd.Persistence;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace FrontEnd
{
    public partial class App : IAsyncDisposable
    {
        private DataSynchronizer? _dbSynchonizer;
        private HubConnection? _hubConnection;
        [Inject] FrontEndAuthenticationStateProvider authentication { get; set; }
        [Inject] NavigationManager navigation { get; set; }
        [Inject] IDbContextFactory<FrontEndDbContext> dbContextFactory { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] IFrontEndApiServices api { get; set; }
        protected override async Task OnInitializedAsync()
        {
            _dbSynchonizer = new DataSynchronizer(js, dbContextFactory, api);
            _dbSynchonizer.OnUpdate += DbChanged;
            dbContext=await _dbSynchonizer.GetPreparedDbContextAsync();
            authentication.AuthenticationStateChanged += Authentication_AuthenticationStateChanged;
            await authentication.RefreshToken();
            StateHasChanged();
        }
        public FrontEndDbContext dbContext { get; private set; }
        private void DbChanged()=> StateHasChanged();
        private async void Authentication_AuthenticationStateChanged(Task<AuthenticationState> task)
        {
            if (authentication.User?.Identity?.IsAuthenticated == true)
            {
                await InitializeNotificationHub();
                await _dbSynchonizer.SynchronizeInBackground();
            }
            else
                await DisposeAsync();
        }
        private async Task InitializeNotificationHub()
        {
            if (_hubConnection != null && _hubConnection.State != HubConnectionState.Disconnected) return;
            var address = navigation.ToAbsoluteUri("/notificationhub");

            _hubConnection = new HubConnectionBuilder()
                //.WithUrl(address)
                .WithUrl(address,
                    options => options.AccessTokenProvider = async () => await authentication.GetJWT())
                .WithAutomaticReconnect()
                .AddMessagePackProtocol()
                .Build();
            _hubConnection.Closed += _hubConnection_Closed;
            _hubConnection.Reconnected += _hubConnection_Reconnected;
            _hubConnection.Reconnecting += _hubConnection_Reconnecting;
            _hubConnection.On<EntitiesModifiedNotification>("ReceiveEntitiesModifiedMessage",
                async (notifications) => {
                    await _dbSynchonizer?.HandleModifiedEntities(authentication?.UserId() ?? Guid.NewGuid(), notifications);
                });
            try
            {
                await _hubConnection.StartAsync();
                var foo = _hubConnection.State;
                await _hubConnection.InvokeAsync("JoinGroup", authentication?.TenantId() ?? Guid.NewGuid());
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
        private async Task DisposeNotificationHub()
        {
            if (_hubConnection is null) return;
            await _hubConnection.InvokeAsync("LeaveGroup", authentication?.TenantId() ?? Guid.NewGuid());
            _hubConnection.Closed -= _hubConnection_Closed;
            _hubConnection.Reconnected -= _hubConnection_Reconnected;
            _hubConnection.Reconnecting -= _hubConnection_Reconnecting;
            await _hubConnection.DisposeAsync();
        }
        private Task _hubConnection_Reconnecting(Exception? arg)
        {
            throw new NotImplementedException();
        }
        private async Task _hubConnection_Reconnected(string? arg)
        {
            await _hubConnection.InvokeAsync("JoinGroup", authentication?.TenantId() ?? Guid.NewGuid());
        }
        private Task _hubConnection_Closed(Exception? arg)
        {
            throw new NotImplementedException();
        }
        public async ValueTask DisposeAsync()
        {
            await DisposeNotificationHub();
            _dbSynchonizer.OnUpdate -= DbChanged;
        }
    }
}
