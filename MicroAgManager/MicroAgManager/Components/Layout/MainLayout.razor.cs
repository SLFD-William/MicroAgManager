using Domain.Logging;
using FrontEnd.Persistence;
using MicroAgManager.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace MicroAgManager.Components.Layout
{
    public partial class MainLayout: LayoutComponentBase,IDisposable
    {
        private static ILogger _log;
        private static FrontEndDbContext _dbContext;
        private AuthenticationStateProvider _authentication;
        private static DataSynchronizer _dbSynchonizer;

        [Inject] DataSynchronizer dbSynchonizer { get; set; }
        [Inject] IConfiguration config { get; set; }
        [Inject] AuthenticationStateProvider authentication { get; set; }
        protected async override Task OnInitializedAsync()
        {
            _authentication = authentication;
            _authentication.AuthenticationStateChanged += Authentication_AuthenticationStateChanged;
            var user = await _authentication.GetAuthenticationStateAsync();
            if (_dbContext == null || _dbSynchonizer == null)
            {
                _dbSynchonizer = dbSynchonizer;
                _dbContext = await _dbSynchonizer.InitializeDbContextAsync();
                if (user.User?.Identity?.IsAuthenticated == true)
                    await _dbSynchonizer.EnsureSynchronizingAsync(null);
                //_log = new DatabaseLoggingProvider(_dbContext, config).CreateLogger("ClientLogging");
            }
        }
        

        private async void Authentication_AuthenticationStateChanged(Task<AuthenticationState> task)
        {
            var user = await task;
            if (user.User?.Identity?.IsAuthenticated == true)
            {
                try
                {
                    Console.WriteLine("Initializing SignalR");
                    //await InitializeNotificationHub();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + e.StackTrace);
                }
                Console.WriteLine("Synchronizing DB");
                await _dbSynchonizer.EnsureSynchronizingAsync(null);
                Console.WriteLine("Synchronized DB");
            }
        }

        public void Dispose()
        {
            _authentication.AuthenticationStateChanged -= Authentication_AuthenticationStateChanged;
        }
    }
}
