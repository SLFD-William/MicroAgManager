using FrontEnd.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;

namespace FrontEnd
{
    public partial class App:IAsyncDisposable
    {
        private DataSynchronizer _dbSynchonizer;
        private HubConnection? _hubConnection;
        
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
                    .WithUrl(address)
                    .Build();

                _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
                {
                    throw new NotImplementedException();
                });

                await _hubConnection.StartAsync();
            }
            else
                await DisposeAsync();
        }
        
        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is not null) await _hubConnection.DisposeAsync();
        }
    }
}
