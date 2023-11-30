using Domain.Context;
using MicroAgManager.Client.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace MicroAgManager.Client
{
    public class ClientApplicationStateProvider:IAsyncDisposable
    {
        private readonly IAPIService _api;
        private readonly DataSynchronizer _dataSynchronizer;
        private NavigationManager _nm;

        public string CurrentURL { get; private set; }
        public bool Initialized { get; private set; } = false;
        public ClientApplicationStateProvider(IAPIService api, DataSynchronizer dataSynchronizer)
        {
            _api = api;
            _dataSynchronizer = dataSynchronizer;
            _dataSynchronizer.OnUpdate += DataSyncUpdate;
            _dataSynchronizer.OnError += DataSyncError;
        }
        public event Action<string>? OnLocationChanged;


        public ValueTask DisposeAsync()
        {
            _dataSynchronizer.OnError -= DataSyncError;
            _dataSynchronizer.OnUpdate -= DataSyncUpdate;
            _nm.LocationChanged -= LocationChanged;
            return ValueTask.CompletedTask;
        }
        public async Task FirstRenderInitialize(IJSRuntime js, NavigationManager nm)
        {
            _nm = nm;
            _nm.LocationChanged += LocationChanged;

            await _dataSynchronizer.InitializeDbContextAsync(js);
            Initialized = true;
        }
        #region Db
        public async Task<FrontEndDbContext> GetDbContextAsync() => await _dataSynchronizer.GetPreparedDbContextAsync();
        private void DataSyncError(Exception exception)
        {
            Console.WriteLine($"{exception.Message} {exception.StackTrace}");
        }

        private void DataSyncUpdate()
        {
            Console.WriteLine("DataSyncUpdate");
        }
        #endregion

        private void LocationChanged(object? sender, LocationChangedEventArgs e)
        {
            CurrentURL = e.Location;
            OnLocationChanged?.Invoke(e.Location);
        }

        public async Task<string> IsServerAlive()=> await _api.TestApiResult();

        
    }
}
