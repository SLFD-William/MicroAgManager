using BackEnd.BusinessLogic.FarmLocation;
using BackEnd.BusinessLogic.LandPlots;
using BackEnd.BusinessLogic.Tenant;
using Domain.Models;
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

        public DataSynchronizer(IJSRuntime js, IDbContextFactory<FrontEndDbContext> dbContextFactory, IFrontEndApiServices api)
        {
            _api = api;
            _dbContextFactory = dbContextFactory;
            _firstTimeSetupTask = FirstTimeSetupAsync(js);
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
        private async Task FirstTimeSetupAsync(IJSRuntime js)
        {
            try { 
                var module = await js.InvokeAsync<IJSObjectReference>("import", "./_content/FrontEnd/dbstorage.js");
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("browser")))
                    await module.InvokeVoidAsync("synchronizeFileWithIndexedDb", SqliteDbFilename);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
 
            using var db = await _dbContextFactory.CreateDbContextAsync();
            await db.Database.MigrateAsync();
            await db.Database.EnsureCreatedAsync();
        }
        private async Task EnsureSynchronizingAsync(List<string>? entityModels)
        {
            // Don't run multiple syncs in parallel. This simple logic is adequate because of single-threadedness.
            if (_isSynchronizing)
                return;
            try
            {
                _isSynchronizing = true;
                SyncCompleted = 0;
                SyncTotal = 0;

                // Get a DB context
                using var db = await GetPreparedDbContextAsync();
                db.ChangeTracker.AutoDetectChangesEnabled = false;
                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                // Begin fetching any updates to the dataset from the backend server
                if (ShouldEntityBeUpdated(entityModels, nameof(TenantModel))) await BulkUpdateTenants(db,_api);
                if (ShouldEntityBeUpdated(entityModels, nameof(FarmLocationModel))) await BulkUpdateFarmLocations(db, _api);
                if (ShouldEntityBeUpdated(entityModels, nameof(LandPlotModel))) await BulkUpdateLandPlots(db, _api);
                OnUpdate?.Invoke();
            }
            catch (Exception ex)
            {
                // TODO: use logger also
                OnError?.Invoke(ex);
            }
            finally
            {
                _isSynchronizing = false;
            }
        }
        private static bool ShouldEntityBeUpdated(List<string>? entityModels, string modelName)
            => !(entityModels?.Any() ?? false) || (entityModels?.Contains(modelName) ?? false);
        private async static Task BulkUpdateTenants(FrontEndDbContext db, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<Guid>(db.Tenants.Select(t=>t.Id));
            var mostRecentUpdate = db.Tenants.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<TenantModel, GetTenantList>("api/GetTenants", new GetTenantList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                foreach (var model in returned.Item2)
                {
                    db.Attach(model);
                    db.Entry(model).State = existingAccountIds.Contains(model.Id) ? EntityState.Modified : EntityState.Added;
                }
                await db.SaveChangesAsync();
            }
        }
        private async static Task BulkUpdateFarmLocations(FrontEndDbContext db, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.Farms.Select(t => t.Id));
            var mostRecentUpdate = db.Farms.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<FarmLocationModel, GetFarmList>("api/GetFarms", new GetFarmList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                foreach (var model in returned.Item2)
                {
                    db.Attach(model);
                    db.Entry(model).State = existingAccountIds.Contains(model.Id) ? EntityState.Modified : EntityState.Added;
                }
                await db.SaveChangesAsync();
            }
        }
        private async static Task BulkUpdateLandPlots(FrontEndDbContext db, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.LandPlots.Select(t => t.Id));
            var mostRecentUpdate = db.LandPlots.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LandPlotModel, GetLandPlotList>("api/GetLandPlots", new GetLandPlotList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                foreach (var model in returned.Item2)
                {
                    db.Attach(model);
                    db.Entry(model).State = existingAccountIds.Contains(model.Id) ? EntityState.Modified : EntityState.Added;
                }
                await db.SaveChangesAsync();
            }
        }
    }
}
