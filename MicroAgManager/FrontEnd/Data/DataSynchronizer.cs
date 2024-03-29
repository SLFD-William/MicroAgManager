﻿using BackEnd.Infrastructure;
using FrontEnd.Persistence;
using FrontEnd.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FrontEnd.Data
{
    public class DataSynchronizer
    {
        public const string SqliteDbFilename = "app.db";
        private readonly Task _firstTimeSetupTask;
        private readonly IDbContextFactory<FrontEndDbContext> _dbContextFactory;
        private bool _isSynchronizing;
        private readonly IFrontEndApiServices _api;
        private readonly IJSRuntime _js;

        public DataSynchronizer(IJSRuntime js, IDbContextFactory<FrontEndDbContext> dbContextFactory, IFrontEndApiServices api)
        {
            _api = api;
            _dbContextFactory = dbContextFactory;
            _js = js;
            _firstTimeSetupTask = FirstTimeSetupAsync();
        }
        public bool DatabaseInitialized { get; private set; } = false;
        public int SyncCompleted { get; private set; }
        public int SyncTotal { get; private set; }
        public event Action? OnUpdate;
        public event Action<Exception>? OnError;
        public async Task<FrontEndDbContext> GetPreparedDbContextAsync()
        {
            await _firstTimeSetupTask;
            DatabaseInitialized = true;
            OnUpdate?.Invoke();
            return await _dbContextFactory.CreateDbContextAsync();
        }
        public async Task SynchronizeInBackground(List<string>? entityModel=null) => await EnsureSynchronizingAsync(entityModel);

        public async Task<string> GetClientDBLink()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Create("browser"))) return string.Empty;
            var module = await _js.InvokeAsync<IJSObjectReference>("import", "./_content/FrontEnd/dbstorage.js");
            await module.InvokeVoidAsync("deleteDBFromCache", SqliteDbFilename);
            await module.InvokeVoidAsync("saveToBrowserCache", SqliteDbFilename);
            var link= await module.InvokeAsync<string>("generateDownloadLink");
            return link ?? string.Empty;
        }

        private async Task FirstTimeSetupAsync()
        {
            try { 
                var module = await _js.InvokeAsync<IJSObjectReference>("import", "./_content/FrontEnd/dbstorage.js");
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
            //await db.Database.EnsureCreatedAsync();
        }
        public async Task HandleModifiedEntities(Guid UserId, EntitiesModifiedNotification notifications)
        { 
            var modified=notifications.EntitiesModified.Select(e=>$"{e.EntityName}Model");
            await EnsureSynchronizingAsync(modified.ToList());
        }
        private async Task EnsureSynchronizingAsync(List<string>? entityModels)
        {
            if (_isSynchronizing)
                while (_isSynchronizing)
                    await Task.Delay(1000);
            try
            {
                _isSynchronizing = true;
                SyncCompleted = 0;
                SyncTotal = 0;

                // Get a DB context
                using var db = await GetPreparedDbContextAsync();
                db.ChangeTracker.AutoDetectChangesEnabled = false;
                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var connection = db.Database.GetDbConnection();
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
                    await DomainFetcher.BulkUpdateBreedingRecords(entityModels, db, connection, _api);
                    await DomainFetcher.BulkUpdateScheduledDuties(entityModels, db, connection, _api);

                    await DomainFetcher.BulkUpdateRegistrations(entityModels, db, connection, _api);
                    await DomainFetcher.BulkUpdateMeasurements(entityModels, db, connection, _api);
                    await DomainFetcher.BulkUpdateTreatmentRecords(entityModels, db, connection, _api);

                    await DomainFetcher.BulkUpdateDutyMilestone(entityModels, db, connection, _api);
                    await DomainFetcher.BulkUpdateDutyEvent(entityModels, db, connection, _api);
                    Console.WriteLine("Ready to Commit");
                    transaction.Commit();
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
