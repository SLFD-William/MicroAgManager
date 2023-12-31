using BackEnd.Infrastructure;
using FrontEnd.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace MicroAgManager.Data
{
    public class DataSynchronizer
    {
        public const string SqliteDbFilename = "app.db";
        private readonly IDbContextFactory<FrontEndDbContext> _dbContextFactory;
        private readonly IAPIService _api;
        private readonly IJSRuntime _js;

        private bool _isSynchronizing;
        private bool _isInitializing;
        public DataSynchronizer(IDbContextFactory<FrontEndDbContext> dbContextFactory, IAPIService api, IJSRuntime js)
        {
            _api = api;
            _dbContextFactory = dbContextFactory;
            _js = js;
            //Task.Run(InitializeDbContextAsync);
        }
        public bool DatabaseInitialized { get; private set; } = false;
        public int SyncCompleted { get; private set; }
        public int SyncTotal { get; private set; }
        public event Action? OnDbInitialized;
        public event Action? OnUpdate;
        public event Action<Exception>? OnError;

        public async Task<FrontEndDbContext> InitializeDbContextAsync()
        {   
            while (_isInitializing)
                await Task.Delay(100);
            _isInitializing = true;
            if (!DatabaseInitialized)
            {
                await FirstTimeSetupAsync();
                DatabaseInitialized = true;
                OnDbInitialized?.Invoke();
                // await EnsureSynchronizingAsync(null);
            }
            _isInitializing = false;
            OnUpdate?.Invoke();
            return await _dbContextFactory.CreateDbContextAsync();
        }

        public async Task<FrontEndDbContext> GetPreparedDbContextAsync()=> await _dbContextFactory.CreateDbContextAsync();
        
        private async Task FirstTimeSetupAsync()
        {
            try
            {
                var module = await _js.InvokeAsync<IJSObjectReference>("import", "./dbstorage.js");
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("browser")))
                {
                    await module.InvokeVoidAsync("deleteDBFromCache", SqliteDbFilename);
                    await module.InvokeVoidAsync("synchronizeFileWithIndexedDb", SqliteDbFilename);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            using var db = await _dbContextFactory.CreateDbContextAsync();
            await db.Database.MigrateAsync();
        }
        public async Task<string> GetClientDBLink()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Create("browser"))) return string.Empty;
            var module = await _js.InvokeAsync<IJSObjectReference>("import", "./dbstorage.js");
            await module.InvokeVoidAsync("deleteDBFromCache", SqliteDbFilename);
            await module.InvokeVoidAsync("saveToBrowserCache", SqliteDbFilename);
            var link = await module.InvokeAsync<string>("generateDownloadLink");
            return link ?? string.Empty;
        }
        public async Task HandleModifiedEntities(Guid UserId, EntitiesModifiedNotification notifications)
        {
            var modified = notifications.EntitiesModified.Select(e => $"{e.EntityName}Model");
            if(!modified.Any()) return;
            await EnsureSynchronizingAsync(modified.ToList());
        }
        public async Task EnsureSynchronizingAsync(List<string>? entityModels)
        {
            if (_isSynchronizing)
                while (_isSynchronizing)
                    await Task.Delay(100);
            try
            {
                _isSynchronizing = true;
                SyncCompleted = 0;
                SyncTotal = 0;
                // Get a DB context
                using (var db = await GetPreparedDbContextAsync())
                { 
                    db.ChangeTracker.AutoDetectChangesEnabled = false;
                    db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    using (var connection = db.Database.GetDbConnection())
                    {
                        connection.Open();
                        using (var transaction = connection.BeginTransaction())
                        {
                            Console.WriteLine("Fetching Data From Server");
                            // Begin fetching any updates to the dataset from the backend server
                            await DomainFetcher.BulkUpdateTenants(entityModels, db, connection, _api);
                            await DomainFetcher.BulkUpdateUnits(entityModels, db, connection, _api);
                            await DomainFetcher.BulkUpdateMeasures(entityModels, db, connection, _api);
                            await DomainFetcher.BulkUpdateTreatments(entityModels, db, connection, _api);
                            await DomainFetcher.BulkUpdateRegistrars(entityModels, db, connection, _api);

                            await DomainFetcher.BulkUpdateFarmLocations(entityModels, db, connection, _api);
                            await DomainFetcher.BulkUpdateLandPlots(entityModels, db, connection, _api);
                            await DomainFetcher.BulkUpdateLivestockAnimals(entityModels, db, connection, _api);
                            await DomainFetcher.BulkUpdateLivestockBreeds(entityModels, db, connection, _api);
                            await DomainFetcher.BulkUpdateLivestockStatuses(entityModels, db, connection, _api);
                            await DomainFetcher.BulkUpdateLivestocks(entityModels, db, connection, _api);
                            await DomainFetcher.BulkUpdateMilestones(entityModels, db, connection, _api);
                            await DomainFetcher.BulkUpdateDuties(entityModels, db, connection, _api);
                            //await DomainFetcher.BulkUpdateChores(entityModels, db, connection, _api);
                            await DomainFetcher.BulkUpdateBreedingRecords(entityModels, db, connection, _api);
                            await DomainFetcher.BulkUpdateScheduledDuties(entityModels, db, connection, _api);

                            await DomainFetcher.BulkUpdateRegistrations(entityModels, db, connection, _api);
                            await DomainFetcher.BulkUpdateMeasurements(entityModels, db, connection, _api);
                            await DomainFetcher.BulkUpdateTreatmentRecords(entityModels, db, connection, _api);

                            await DomainFetcher.BulkUpdateDutyMilestone(entityModels, db, connection, _api);
                            await DomainFetcher.BulkUpdateDutyEvent(entityModels, db, connection, _api);
                            //await DomainFetcher.BulkUpdateDutyChore(entityModels, db, connection, _api);

                            transaction.Commit();
                        }
                    }
                }
                OnUpdate?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.StackTrace}");
                OnError?.Invoke(ex);
            }
            finally
            {
                _isSynchronizing = false;
            }
        }
    }
}
