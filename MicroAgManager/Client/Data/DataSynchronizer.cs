using BackEnd.BusinessLogic.Duty;
using BackEnd.BusinessLogic.Event;
using BackEnd.BusinessLogic.FarmLocation;
using BackEnd.BusinessLogic.LandPlots;
using BackEnd.BusinessLogic.Livestock.Breeds;
using BackEnd.BusinessLogic.Livestock.Status;
using BackEnd.BusinessLogic.Livestock.Types;
using BackEnd.BusinessLogic.LivestockFeed;
using BackEnd.BusinessLogic.Milestone;
using BackEnd.BusinessLogic.ScheduledDuty;
using BackEnd.BusinessLogic.Tenant;
using BackEnd.Models;
using Domain.Models;
using FrontEnd.Pages;
using FrontEnd.Persistence;
using FrontEnd.Services;
using MediatR;
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
        public async Task HandleModifiedEntities(Guid UserId, EntitiesModifiedNotification notifications)
        { 
            var modified=notifications.EntitiesModified.Where(e=>e.ModifiedBy!=UserId || e.Modification=="Created")
                .Select(e=>$"{e.EntityName}Model");
            await EnsureSynchronizingAsync(modified.ToList());
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
                if (ShouldEntityBeUpdated(entityModels, nameof(LivestockTypeModel))) await BulkUpdateLivestockTypes(db, _api);
                if (ShouldEntityBeUpdated(entityModels, nameof(LivestockBreedModel))) await BulkUpdateLivestockBreeds(db, _api);
                if (ShouldEntityBeUpdated(entityModels, nameof(LivestockStatusModel))) await BulkUpdateLivestockStatuses(db, _api);
                if (ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedModel))) await BulkUpdateLivestockFeeds(db, _api);
                if (ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedServingModel))) await BulkUpdateLivestockFeedServings(db, _api);
                if (ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedDistributionModel))) await BulkUpdateLivestockFeedDistributions(db, _api);
                if (ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedAnalysisParameterModel))) await BulkUpdateLivestockFeedAnalysisParameters(db, _api);
                if (ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedAnalysisModel))) await BulkUpdateLivestockFeedAnalyses(db, _api);
                if (ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedAnalysisResultModel))) await BulkUpdateLivestockFeedAnalysisResults(db, _api);
                if (ShouldEntityBeUpdated(entityModels, nameof(DutyModel))) await BulkUpdateDuties(db, _api);
                if (ShouldEntityBeUpdated(entityModels, nameof(ScheduledDutyModel))) await BulkUpdateScheduledDuties(db, _api);
                if (ShouldEntityBeUpdated(entityModels, nameof(MilestoneModel))) await BulkUpdateMilestones(db, _api);
                if (ShouldEntityBeUpdated(entityModels, nameof(EventModel))) await BulkUpdateEvents(db, _api);
                await db.SaveChangesAsync();
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
        private async static Task BulkUpdateLivestockTypes(FrontEndDbContext db, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.LivestockTypes.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockTypes.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockTypeModel, GetLivestockTypeList>("api/GetLivestockTypes", new GetLivestockTypeList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                foreach (var model in returned.Item2)
                {
                    db.Attach(model);
                    db.Entry(model).State = existingAccountIds.Contains(model.Id) ? EntityState.Modified : EntityState.Added;
                }
            }
        }
        private async static Task BulkUpdateLivestockBreeds(FrontEndDbContext db, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.LivestockBreeds.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockBreeds.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockBreedModel, GetLivestockBreedList>("api/GetLivestockBreeds", new GetLivestockBreedList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                foreach (var model in returned.Item2)
                {
                    db.Attach(model);
                    db.Entry(model).State = existingAccountIds.Contains(model.Id) ? EntityState.Modified : EntityState.Added;
                }
            }
        }
        private async static Task BulkUpdateLivestockStatuses(FrontEndDbContext db, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.LivestockStatuses.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockStatuses.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockStatusModel, GetLivestockStatusList>("api/GetLivestockStatuses", new GetLivestockStatusList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                foreach (var model in returned.Item2)
                {
                    db.Attach(model);
                    db.Entry(model).State = existingAccountIds.Contains(model.Id) ? EntityState.Modified : EntityState.Added;
                }
            }
        }
        private async static Task BulkUpdateLivestockFeeds(FrontEndDbContext db, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.LivestockFeeds.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockFeeds.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockFeedModel, GetLivestockFeedList>("api/GetLivestockFeeds", new GetLivestockFeedList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                foreach (var model in returned.Item2)
                {
                    db.Attach(model);
                    db.Entry(model).State = existingAccountIds.Contains(model.Id) ? EntityState.Modified : EntityState.Added;
                }
            }
        }
        private async static Task BulkUpdateLivestockFeedServings(FrontEndDbContext db, IFrontEndApiServices api)
        { 
            var existingAccountIds = new HashSet<long>(db.LivestockFeedServings.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockFeedServings.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockFeedServingModel, GetLivestockFeedServingList>("api/GetLivestockFeedServings", new GetLivestockFeedServingList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                foreach (var model in returned.Item2)
                {
                    db.Attach(model);
                    db.Entry(model).State = existingAccountIds.Contains(model.Id) ? EntityState.Modified : EntityState.Added;
                }
            }
        }
        private async static Task BulkUpdateLivestockFeedDistributions(FrontEndDbContext db, IFrontEndApiServices api)
        { 
            var existingAccountIds = new HashSet<long>(db.LivestockFeedDistributions.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockFeedDistributions.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockFeedDistributionModel, GetLivestockFeedDistributionList>("api/GetLivestockFeedDistributions", new GetLivestockFeedDistributionList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                foreach (var model in returned.Item2)
                {
                    db.Attach(model);
                    db.Entry(model).State = existingAccountIds.Contains(model.Id) ? EntityState.Modified : EntityState.Added;
                }
            }
        }
        private async static Task BulkUpdateLivestockFeedAnalysisParameters(FrontEndDbContext db, IFrontEndApiServices api)
        { 
            var existingAccountIds = new HashSet<long>(db.LivestockFeedAnalysisParameters.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockFeedAnalysisParameters.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockFeedAnalysisParameterModel, GetLivestockFeedAnalysisParameterList>("api/GetLivestockFeedAnalysisParameters", new GetLivestockFeedAnalysisParameterList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                foreach (var model in returned.Item2)
                {
                    db.Attach(model);
                    db.Entry(model).State = existingAccountIds.Contains(model.Id) ? EntityState.Modified : EntityState.Added;
                }
            }
        }
        private async static Task BulkUpdateLivestockFeedAnalyses(FrontEndDbContext db, IFrontEndApiServices api)
        { 
            var existingAccountIds = new HashSet<long>(db.LivestockFeedAnalyses.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockFeedAnalyses.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockFeedAnalysisModel, GetLivestockFeedAnalysisList>("api/GetLivestockFeedAnalyses", new GetLivestockFeedAnalysisList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                foreach (var model in returned.Item2)
                {
                    db.Attach(model);
                    db.Entry(model).State = existingAccountIds.Contains(model.Id) ? EntityState.Modified : EntityState.Added;
                }
            }
        }
        private async static Task BulkUpdateLivestockFeedAnalysisResults(FrontEndDbContext db, IFrontEndApiServices api)
        { 
            var existingAccountIds = new HashSet<long>(db.LivestockFeedAnalysisResults.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockFeedAnalysisResults.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockFeedAnalysisResultModel, GetLivestockFeedAnalysisResultList>("api/GetLivestockFeedAnalysisResults", new GetLivestockFeedAnalysisResultList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                foreach (var model in returned.Item2)
                {
                    db.Attach(model);
                    db.Entry(model).State = existingAccountIds.Contains(model.Id) ? EntityState.Modified : EntityState.Added;
                }
            }
        }
        private async static Task BulkUpdateDuties(FrontEndDbContext db, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.Duties.Select(t => t.Id));
            var mostRecentUpdate = db.Duties.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<DutyModel, GetDutyList>("api/GetDuties", new GetDutyList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                foreach (var model in returned.Item2)
                {
                    db.Attach(model);
                    db.Entry(model).State = existingAccountIds.Contains(model.Id) ? EntityState.Modified : EntityState.Added;
                }
            }
        }
        private async static Task BulkUpdateScheduledDuties(FrontEndDbContext db, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.ScheduledDuties.Select(t => t.Id));
            var mostRecentUpdate = db.ScheduledDuties.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<ScheduledDutyModel, GetScheduledDutyList>("api/GetScheduledDuties", new GetScheduledDutyList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                foreach (var model in returned.Item2)
                {
                    db.Attach(model);
                    db.Entry(model).State = existingAccountIds.Contains(model.Id) ? EntityState.Modified : EntityState.Added;
                }
            }
        }
        private async static Task BulkUpdateMilestones(FrontEndDbContext db, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.ScheduledDuties.Select(t => t.Id));
            var mostRecentUpdate = db.Milestones.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<MilestoneModel, GetMilestoneList>("api/GetMilestones", new GetMilestoneList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                foreach (var model in returned.Item2)
                {
                    db.Attach(model);
                    db.Entry(model).State = existingAccountIds.Contains(model.Id) ? EntityState.Modified : EntityState.Added;
                }
            }
        }
        private async static Task BulkUpdateEvents(FrontEndDbContext db, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.ScheduledDuties.Select(t => t.Id));
            var mostRecentUpdate = db.Events.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<EventModel, GetEventList>("api/GetEvents", new GetEventList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                foreach (var model in returned.Item2)
                {
                    db.Attach(model);
                    db.Entry(model).State = existingAccountIds.Contains(model.Id) ? EntityState.Modified : EntityState.Added;
                }
            }
        }
    }
}
