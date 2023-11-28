using FrontEnd.Persistence;
using MicroAgManager.Client.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace MicroAgManager.Client
{
    public class ClientApplicationStateProvider
    {
        private static readonly string AppInstance="StaticAlive";
        public string AppInstanceName { get; private set; } = AppInstance;
        public string AppStaticInstanceName { get; private set; } = AppInstance;
        private static HttpClient _http;
        private readonly IJSRuntime _js;
        private readonly IAPIService _api;

        private readonly DataSynchronizer _dataSynchronizer;
        public ClientApplicationStateProvider(IJSRuntime js, HttpClient http, IDbContextFactory<FrontEndDbContext> dbContextFactory, IConfiguration config)
        {
            _http = http;
            AppInstanceName = "InstanceAlive";
            _api = new APIService(_http,config);
            _dataSynchronizer =new DataSynchronizer(js, dbContextFactory, _api);
        }
        public async Task<FrontEndDbContext> GetDbContextAsync()=> await _dataSynchronizer.GetPreparedDbContextAsync();
        public bool IsDbInitialized { get => _dataSynchronizer.DatabaseInitialized; }
        public async Task<string> IsServerAlive()=> await _api.TestApiResult();
    }
}
