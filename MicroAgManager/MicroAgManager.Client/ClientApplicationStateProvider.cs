using FrontEnd.Persistence;
using MicroAgManager.Client.Data;
using Microsoft.JSInterop;

namespace MicroAgManager.Client
{
    public class ClientApplicationStateProvider
    {
        private static readonly string AppInstance="StaticAlive";
        public string AppInstanceName { get; private set; } = AppInstance;
        public string AppStaticInstanceName { get; private set; } = AppInstance;
        private readonly IAPIService _api;

        private readonly DataSynchronizer _dataSynchronizer;
        public ClientApplicationStateProvider(IAPIService api, DataSynchronizer dataSynchronizer)
        {
            AppInstanceName = "InstanceAlive";
            _api = api;
            _dataSynchronizer = dataSynchronizer;
        }
        public async Task<FrontEndDbContext> InitializeDbContextAsync(IJSRuntime js) => await _dataSynchronizer.InitializeDbContextAsync(js);
        public async Task<FrontEndDbContext> GetDbContextAsync()=> await _dataSynchronizer.GetPreparedDbContextAsync();
        public bool IsDbInitialized { get => _dataSynchronizer.DatabaseInitialized; }
        public async Task<string> IsServerAlive()=> await _api.TestApiResult();
    }
}
