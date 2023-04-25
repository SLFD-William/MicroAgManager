using FrontEnd.Data;
using FrontEnd.Pages;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace FrontEnd
{
    public partial class App : IAsyncDisposable
    {
        private DataSynchronizer _dbSynchonizer;
        private HubConnection? _hubConnection;
        [Inject] FrontEndAuthenticationStateProvider authentication { get; set; }
        [Inject] NavigationManager navigation { get; set; }
        [Inject] IDbContextFactory<FrontEndDbContext> dbContextFactory { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] HttpClient httpClient { get; set; }
        public App( )
        {
            _dbSynchonizer = new DataSynchronizer(js, dbContextFactory);
            _dbSynchonizer.OnUpdate += DbChanged;
        }
        protected override async Task OnInitializedAsync()
        {
            authentication.AuthenticationStateChanged += Authentication_AuthenticationStateChanged;
            await authentication.RefreshToken();
        }
        private void DbChanged()=> StateHasChanged();
        private async void Authentication_AuthenticationStateChanged(Task<AuthenticationState> task)
        {
            if (authentication.User?.Identity?.IsAuthenticated == true)
            {
                if (_hubConnection!=null && _hubConnection.State != HubConnectionState.Disconnected) return;
                var address = navigation.ToAbsoluteUri("/notificationhub");

                _hubConnection = new HubConnectionBuilder()
                    //.WithUrl(address)
                    .WithUrl(address,
                        options => options.AccessTokenProvider =  async () => await authentication.GetJWT())
                    .WithAutomaticReconnect()
                    .AddMessagePackProtocol()
                    .Build();
                _hubConnection.Closed += _hubConnection_Closed;
                _hubConnection.Reconnected += _hubConnection_Reconnected;
                _hubConnection.Reconnecting += _hubConnection_Reconnecting;
                _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
                {
                    throw new NotImplementedException();
                });
                try
                {
                    await _hubConnection.StartAsync();
                    var foo = _hubConnection.State;
                }
                catch(Exception ex) { Console.WriteLine(ex); }
                
            }
            else
                await DisposeAsync();
        }

        private Task _hubConnection_Reconnecting(Exception? arg)
        {
            throw new NotImplementedException();
        }

        private Task _hubConnection_Reconnected(string? arg)
        {
            throw new NotImplementedException();
        }

        private Task _hubConnection_Closed(Exception? arg)
        {
            throw new NotImplementedException();
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is null) return;
            _hubConnection.Closed -= _hubConnection_Closed;
            _hubConnection.Reconnected -= _hubConnection_Reconnected;
            _hubConnection.Reconnecting -= _hubConnection_Reconnecting;
            await _hubConnection.DisposeAsync();
        }
    }
}
