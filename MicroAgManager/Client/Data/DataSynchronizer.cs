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
using BackEnd.Infrastructure;
using Domain.Abstracts;
using Domain.Models;
using FrontEnd.Persistence;
using FrontEnd.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Data.Common;
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
        static DbParameter AddNamedParameter(DbCommand command, string name)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            command.Parameters.Add(parameter);
            return parameter;
        }
        private static Dictionary<string,DbParameter> GetBaseModelParameters(DbCommand command)=>
            new Dictionary<string,DbParameter>
            {
                {"Id", AddNamedParameter(command, "$Id") },
                {"Deleted", AddNamedParameter(command, "$Deleted") },
                {"EntityModifiedOn",AddNamedParameter(command, "$EntityModifiedOn") },
                {"ModifiedOn",AddNamedParameter(command, "$ModifiedOn") },
                {"ModifiedBy",AddNamedParameter(command, "$ModifiedBy") }
            };

        private static void PopulateBaseModelParameters(Dictionary<string, DbParameter> parameters, BaseModel model)
        {
            parameters["Id"].Value = model.Id;
            parameters["Deleted"].Value = model.Deleted;
            parameters["EntityModifiedOn"].Value = model.EntityModifiedOn;
            parameters["ModifiedOn"].Value = model.ModifiedOn;
            parameters["ModifiedBy"].Value = model.ModifiedBy;
        }
        private async Task EnsureSynchronizingAsync(List<string>? entityModels)
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
                using var db = await GetPreparedDbContextAsync();
                db.ChangeTracker.AutoDetectChangesEnabled = false;
                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var connection = db.Database.GetDbConnection();
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // Begin fetching any updates to the dataset from the backend server
                    if (ShouldEntityBeUpdated(entityModels, nameof(TenantModel))) await BulkUpdateTenants(db,connection, _api);
                    if (ShouldEntityBeUpdated(entityModels, nameof(FarmLocationModel))) await BulkUpdateFarmLocations(db,connection, _api);
                    if (ShouldEntityBeUpdated(entityModels, nameof(LandPlotModel))) await BulkUpdateLandPlots(db, connection, _api);
                    if (ShouldEntityBeUpdated(entityModels, nameof(LivestockTypeModel))) await BulkUpdateLivestockTypes(db, connection, _api);
                    if (ShouldEntityBeUpdated(entityModels, nameof(LivestockBreedModel))) await BulkUpdateLivestockBreeds(db, connection, _api);
                    if (ShouldEntityBeUpdated(entityModels, nameof(LivestockStatusModel))) await BulkUpdateLivestockStatuses(db, connection, _api);
                    if (ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedModel))) await BulkUpdateLivestockFeeds(db, connection, _api);
                    if (ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedServingModel))) await BulkUpdateLivestockFeedServings(db, connection, _api);
                    if (ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedDistributionModel))) await BulkUpdateLivestockFeedDistributions(db, connection, _api);
                    if (ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedAnalysisParameterModel))) await BulkUpdateLivestockFeedAnalysisParameters(db, connection, _api);
                    if (ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedAnalysisModel))) await BulkUpdateLivestockFeedAnalyses(db, connection, _api);
                    if (ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedAnalysisResultModel))) await BulkUpdateLivestockFeedAnalysisResults(db, connection, _api);
                    if (ShouldEntityBeUpdated(entityModels, nameof(DutyModel))) await BulkUpdateDuties(db, connection, _api);
                    if (ShouldEntityBeUpdated(entityModels, nameof(ScheduledDutyModel))) await BulkUpdateScheduledDuties(db, connection, _api);
                    if (ShouldEntityBeUpdated(entityModels, nameof(MilestoneModel))) await BulkUpdateMilestones(db, connection, _api);
                    if (ShouldEntityBeUpdated(entityModels, nameof(EventModel))) await BulkUpdateEvents(db, connection, _api);
                    transaction.Commit();
                }
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
        private async static Task BulkUpdateTenants(FrontEndDbContext db,DbConnection connection, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<Guid>(db.Tenants.Select(t=>t.GuidId));
            var mostRecentUpdate = db.Tenants.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<TenantModel, GetTenantList>("api/GetTenants", new GetTenantList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var name = AddNamedParameter(command, "$Name");
                var guidId = AddNamedParameter(command, "$GuidId");
                var tenantUserAdminId = AddNamedParameter(command, "$TenantUserAdminId");
                command.CommandText = $"INSERT or REPLACE INTO Tenants (Id,GuidId,Name,TenantUserAdminId, Deleted,EntityModifiedOn,ModifiedOn,ModifiedBy) " +
                    $"Values ({baseParameters["Id"].ParameterName},{guidId.ParameterName},{name.ParameterName},{tenantUserAdminId.ParameterName},{baseParameters["Deleted"].ParameterName}," +
                    $"{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName})";
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    name.Value = model.Name;
                    guidId.Value = model.GuidId;
                    tenantUserAdminId.Value = model.TenantUserAdminId;
                    await command.ExecuteNonQueryAsync();
                }
             }
        }
        private async static Task BulkUpdateFarmLocations(FrontEndDbContext db, DbConnection connection, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.Farms.Select(t => t.Id));
            var mostRecentUpdate = db.Farms.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<FarmLocationModel, GetFarmList>("api/GetFarms", new GetFarmList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);

                var tenantId = AddNamedParameter(command, "$TenantId");
                var name = AddNamedParameter(command, "$Name");
                var longitude = AddNamedParameter(command, "$Longitude");
                var latitude = AddNamedParameter(command, "$Latitude");
                var streetAddress = AddNamedParameter(command, "$StreetAddress");
                var city = AddNamedParameter(command, "$City");
                var state = AddNamedParameter(command, "$State");
                var zipCode = AddNamedParameter(command, "$Zip");
                var country = AddNamedParameter(command, "$Country");

                command.CommandText = $"INSERT or REPLACE INTO Farms (Id,Deleted,EntityModifiedOn,ModifiedOn,ModifiedBy,TenantId,Name,Longitude,Latitude,StreetAddress,City,State,Zip,Country) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                    $"{tenantId.ParameterName},{name.ParameterName},{longitude.ParameterName},{latitude.ParameterName},{streetAddress.ParameterName},{city.ParameterName},{state.ParameterName},{zipCode.ParameterName},{country.ParameterName})";
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    baseParameters["Id"].Value = model.Id;
                    baseParameters["Deleted"].Value = model.Deleted;
                    baseParameters["EntityModifiedOn"].Value = model.EntityModifiedOn;
                    baseParameters["ModifiedOn"].Value = model.ModifiedOn;
                    baseParameters["ModifiedBy"].Value = model.ModifiedBy;
                    tenantId.Value = model.TenantId;
                    name.Value = model.Name;
                    longitude.Value = model.Longitude ?? string.Empty;
                    latitude.Value = model.Latitude ?? string.Empty;
                    streetAddress.Value = model.StreetAddress ?? string.Empty;
                    city.Value = model.City ?? string.Empty;
                    state.Value = model.State ?? string.Empty;
                    zipCode.Value = model.Zip ?? string.Empty;
                    country.Value = model.Country ?? string.Empty;
                    await command.ExecuteNonQueryAsync();
                }





            }
        }
        private async static Task BulkUpdateLandPlots(FrontEndDbContext db, DbConnection connection, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.LandPlots.Select(t => t.Id));
            var mostRecentUpdate = db.LandPlots.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LandPlotModel, GetLandPlotList>("api/GetLandPlots", new GetLandPlotList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
               
                var farmLocationId = AddNamedParameter(command, "$FarmLocationId");
                var name = AddNamedParameter(command, "$Name");
                var description = AddNamedParameter(command, "$Description");
                var area = AddNamedParameter(command, "$Area");
                var areaUnit = AddNamedParameter(command, "$AreaUnit");
                var usage= AddNamedParameter(command, "$Usage");
                var parentPlotId= AddNamedParameter(command, "$ParentPlotId");

                command.CommandText = $"INSERT or REPLACE INTO LandPlots (Id,Deleted,EntityModifiedOn,ModifiedOn,ModifiedBy,FarmLocationId,Name,Description,Area,AreaUnit,Usage,ParentPlotId) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}" +
                    $",{farmLocationId.ParameterName},{name.ParameterName},{description.ParameterName},{area.ParameterName},{areaUnit.ParameterName},{usage.ParameterName},{parentPlotId.ParameterName})";
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    farmLocationId.Value = model.FarmLocationId;
                    name.Value = model.Name;
                    description.Value = model.Description;
                    area.Value = model.Area;
                    areaUnit.Value = model.AreaUnit;
                    usage.Value = model.Usage;
                   
                    parentPlotId.Value = model.ParentPlotId.HasValue ? model.ParentPlotId.Value :DBNull.Value;
                    await command.ExecuteNonQueryAsync();
                }


            }
        }
        private async static Task BulkUpdateLivestockTypes(FrontEndDbContext db,DbConnection connection, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.LivestockTypes.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockTypes.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockTypeModel, GetLivestockTypeList>("api/GetLivestockTypes", new GetLivestockTypeList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var name = AddNamedParameter(command, "$Name");
                var groupName = AddNamedParameter(command, "$GroupName");
                var parentMaleName = AddNamedParameter(command, "$ParentMaleName");
                var parentFemaleName = AddNamedParameter(command, "$ParentFemaleName");
                var care= AddNamedParameter(command, "$Care");

                command.CommandText = $"INSERT or REPLACE INTO LivestockTypes (Id,Deleted,EntityModifiedOn,ModifiedOn,ModifiedBy,Name,GroupName,ParentMaleName,ParentFemaleName,Care) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                    $"{name.ParameterName},{groupName.ParameterName},{parentMaleName.ParameterName},{parentFemaleName.ParameterName},{care.ParameterName})";
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    name.Value = model.Name;
                    groupName.Value = model.GroupName;
                    parentMaleName.Value = model.ParentMaleName;
                    parentFemaleName.Value = model.ParentFemaleName;
                    care.Value = model.Care;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        private async static Task BulkUpdateLivestockBreeds(FrontEndDbContext db, DbConnection connection, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.LivestockBreeds.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockBreeds.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockBreedModel, GetLivestockBreedList>("api/GetLivestockBreeds", new GetLivestockBreedList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var livestockTypeId = AddNamedParameter(command, "$LivestockTypeId");
                var name = AddNamedParameter(command, "$Name");
                var emojiChar = AddNamedParameter(command, "$EmojiChar");
                var gestationPeriod= AddNamedParameter(command, "$GestationPeriod");
                var heatPeriod= AddNamedParameter(command, "$HeatPeriod");

                command.CommandText = $"INSERT or REPLACE INTO LivestockBreeds (Id,Deleted,EntityModifiedOn,ModifiedOn,ModifiedBy,LivestockTypeId,Name,EmojiChar,GestationPeriod,HeatPeriod) " +
                    $"Values ({baseParameters["Id"].ParameterName} , {baseParameters["Deleted"].ParameterName} , {baseParameters["EntityModifiedOn"].ParameterName} , {baseParameters["ModifiedOn"].ParameterName} , {baseParameters["ModifiedBy"].ParameterName}," +
                    $"{livestockTypeId.ParameterName},{name.ParameterName},{emojiChar.ParameterName},{gestationPeriod.ParameterName},{heatPeriod.ParameterName})";
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    livestockTypeId.Value = model.LivestockTypeId;
                    name.Value = model.Name;
                    emojiChar.Value = model.EmojiChar;
                    gestationPeriod.Value = model.GestationPeriod;
                    heatPeriod.Value = model.HeatPeriod;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        private async static Task BulkUpdateLivestockStatuses(FrontEndDbContext db, DbConnection connection, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.LivestockStatuses.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockStatuses.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockStatusModel, GetLivestockStatusList>("api/GetLivestockStatuses", new GetLivestockStatusList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var livestockTypeId = AddNamedParameter(command, "$LivestockTypeId");
                var status = AddNamedParameter(command, "$Status");
                var defaultStatus= AddNamedParameter(command, "$DefaultStatus");
                var beingManaged= AddNamedParameter(command, "$BeingManaged");
                var sterile = AddNamedParameter(command, "$Sterile");
                var inMilk = AddNamedParameter(command, "$InMilk");
                var bottleFed = AddNamedParameter(command, "$BottleFed");
                var forSale = AddNamedParameter(command, "$ForSale");

                command.CommandText = $"INSERT or REPLACE INTO LivestockStatuses (Id,Deleted,EntityModifiedOn,ModifiedOn,ModifiedBy,LivestockTypeId,Status,DefaultStatus,BeingManaged,Sterile,InMilk,BottleFed,ForSale) " +
                    $"Values ({baseParameters["Id"].ParameterName} , {baseParameters["Deleted"].ParameterName} , {baseParameters["EntityModifiedOn"].ParameterName} , {baseParameters["ModifiedOn"].ParameterName} , {baseParameters["ModifiedBy"].ParameterName}," +
                    $"{livestockTypeId.ParameterName},{status.ParameterName},{defaultStatus.ParameterName},{beingManaged.ParameterName},{sterile.ParameterName},{inMilk.ParameterName},{bottleFed.ParameterName},{forSale.ParameterName})";
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    livestockTypeId.Value = model.LivestockTypeId;
                    status.Value = model.Status;
                    defaultStatus.Value = model.DefaultStatus;
                    beingManaged.Value = model.BeingManaged;
                    sterile.Value = model.Sterile;
                    inMilk.Value = model.InMilk;
                    bottleFed.Value = model.BottleFed;
                    forSale.Value = model.ForSale;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        private async static Task BulkUpdateLivestockFeeds(FrontEndDbContext db, DbConnection connection, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.LivestockFeeds.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockFeeds.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockFeedModel, GetLivestockFeedList>("api/GetLivestockFeeds", new GetLivestockFeedList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var livestockTypeId = AddNamedParameter(command, "$LivestockTypeId");
                var name = AddNamedParameter(command, "$Name");
                var source = AddNamedParameter(command, "$Source");
                var cutting = AddNamedParameter(command, "$Cutting");
                var active = AddNamedParameter(command, "$Active");
                var quantity = AddNamedParameter(command, "$Quantity");
                var quantityUnit = AddNamedParameter(command, "$QuantityUnit");
                var quantityWarning = AddNamedParameter(command, "$QuantityWarning");
                var feedType = AddNamedParameter(command, "$FeedType");
                var distribution = AddNamedParameter(command, "$Distribution");

                command.CommandText = $"INSERT or REPLACE INTO LivestockFeeds (Id,Deleted,EntityModifiedOn,ModifiedOn,ModifiedBy,LivestockTypeId,Name,Source,Cutting,Active,Quantity,QuantityUnit,QuantityWarning,FeedType,Distribution) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                    $"{livestockTypeId.ParameterName},{name.ParameterName},{source.ParameterName},{cutting.ParameterName},{active.ParameterName},{quantity.ParameterName},{quantityUnit.ParameterName},{quantityWarning.ParameterName},{feedType.ParameterName},{distribution.ParameterName})";
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    livestockTypeId.Value = model.LivestockTypeId;
                    name.Value = model.Name;
                    source.Value = model.Source;

                    cutting.Value = model.Cutting.HasValue ? model.Cutting : DBNull.Value;
                    active.Value = model.Active;
                    quantity.Value = model.Quantity;
                    quantityUnit.Value = model.QuantityUnit;
                    quantityWarning.Value = model.QuantityWarning;
                    feedType.Value = model.FeedType;
                    distribution.Value = model.Distribution;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        private async static Task BulkUpdateLivestockFeedServings(FrontEndDbContext db, DbConnection connection, IFrontEndApiServices api)
        { 
            var existingAccountIds = new HashSet<long>(db.LivestockFeedServings.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockFeedServings.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockFeedServingModel, GetLivestockFeedServingList>("api/GetLivestockFeedServings", new GetLivestockFeedServingList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var feedId= AddNamedParameter(command, "$FeedId");
                var statusId= AddNamedParameter(command, "$StatusId");
                var servingFrequency = AddNamedParameter(command, "$ServingFrequency");
                var serving = AddNamedParameter(command, "$Serving");

                command.CommandText = $"INSERT or REPLACE INTO LivestockFeedServings (Id,Deleted,EntityModifiedOn,ModifiedOn,ModifiedBy,FeedId,StatusId,ServingFrequency,Serving) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                    $"{feedId.ParameterName},{statusId.ParameterName},{servingFrequency.ParameterName},{serving.ParameterName})";
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    feedId.Value = model.FeedId;
                    statusId.Value = model.StatusId;
                    servingFrequency.Value = model.ServingFrequency;
                    serving.Value = model.Serving;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        private async static Task BulkUpdateLivestockFeedDistributions(FrontEndDbContext db, DbConnection connection, IFrontEndApiServices api)
        { 
            var existingAccountIds = new HashSet<long>(db.LivestockFeedDistributions.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockFeedDistributions.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockFeedDistributionModel, GetLivestockFeedDistributionList>("api/GetLivestockFeedDistributions", new GetLivestockFeedDistributionList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var feedId= AddNamedParameter(command, "$FeedId");
                var quantity = AddNamedParameter(command, "$Quantity");
                var discarded = AddNamedParameter(command, "$Discarded");
                var note = AddNamedParameter(command, "$Note");
                var datePerformed = AddNamedParameter(command, "$DatePerformed");

                command.CommandText = $"INSERT or REPLACE INTO LivestockFeedDistributions (Id,Deleted,EntityModifiedOn,ModifiedOn,ModifiedBy,FeedId,Quantity,Discarded,Note,DatePerformed) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                    $"{feedId.ParameterName},{quantity.ParameterName},{discarded.ParameterName},{note.ParameterName},{datePerformed.ParameterName})";
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    feedId.Value = model.FeedId;
                    quantity.Value = model.Quantity;
                    discarded.Value = model.Discarded.HasValue ? model.Discarded : DBNull.Value;
                    note.Value = model.Note;
                    datePerformed.Value = model.DatePerformed;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        private async static Task BulkUpdateLivestockFeedAnalysisParameters(FrontEndDbContext db, DbConnection connection, IFrontEndApiServices api)
        { 
            var existingAccountIds = new HashSet<long>(db.LivestockFeedAnalysisParameters.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockFeedAnalysisParameters.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockFeedAnalysisParameterModel, GetLivestockFeedAnalysisParameterList>("api/GetLivestockFeedAnalysisParameters", new GetLivestockFeedAnalysisParameterList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var parameter= AddNamedParameter(command, "$Parameter");
                var subParameter = AddNamedParameter(command, "$SubParameter");
                var unit = AddNamedParameter(command, "$Unit");
                var method = AddNamedParameter(command, "$Method");
                var reportOrder = AddNamedParameter(command, "$ReportOrder");

                command.CommandText = $"INSERT or REPLACE INTO LivestockFeedAnalysisParameters (Id,Deleted,EntityModifiedOn,ModifiedOn,ModifiedBy,Parameter,SubParameter,Unit,Method,ReportOrder) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                    $"{parameter.ParameterName},{subParameter.ParameterName},{unit.ParameterName},{method.ParameterName},{reportOrder.ParameterName})";
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    parameter.Value = model.Parameter;
                    subParameter.Value = model.SubParameter;
                    unit.Value = model.Unit;
                    method.Value = model.Method;
                    reportOrder.Value = model.ReportOrder;
                    await command.ExecuteNonQueryAsync();
                }
               
            }
        }
        private async static Task BulkUpdateLivestockFeedAnalyses(FrontEndDbContext db, DbConnection connection, IFrontEndApiServices api)
        { 
            var existingAccountIds = new HashSet<long>(db.LivestockFeedAnalyses.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockFeedAnalyses.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockFeedAnalysisModel, GetLivestockFeedAnalysisList>("api/GetLivestockFeedAnalyses", new GetLivestockFeedAnalysisList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var feedId= AddNamedParameter(command, "$FeedId");
                var labNumber = AddNamedParameter(command, "$LabNumber");
                var testCode = AddNamedParameter(command, "$TestCode");
                var dateSampled = AddNamedParameter(command, "$DateSampled");
                var dateReceived = AddNamedParameter(command, "$DateReceived");
                var dateReported = AddNamedParameter(command, "$DateReported");
                var datePrinted = AddNamedParameter(command, "$DatePrinted");

                command.CommandText = $"INSERT or REPLACE INTO LivestockFeedAnalyses (Id,Deleted,EntityModifiedOn,ModifiedOn,ModifiedBy,FeedId,LabNumber,TestCode,DateSampled,DateReceived,DateReported,DatePrinted) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                    $"{feedId.ParameterName},{labNumber.ParameterName},{testCode.ParameterName},{dateSampled.ParameterName},{dateReceived.ParameterName},{dateReported.ParameterName},{datePrinted.ParameterName})";

                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    feedId.Value = model.FeedId;
                    labNumber.Value = model.LabNumber;
                    testCode.Value = model.TestCode;
                    dateSampled.Value = model.DateSampled.HasValue ? model.DateSampled : DBNull.Value;
                    dateReceived.Value = model.DateReceived.HasValue ? model.DateReceived : DBNull.Value;
                    dateReported.Value = model.DateReported.HasValue ? model.DateReported : DBNull.Value;
                    datePrinted.Value = model.DatePrinted.HasValue ? model.DatePrinted : DBNull.Value;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        private async static Task BulkUpdateLivestockFeedAnalysisResults(FrontEndDbContext db, DbConnection connection, IFrontEndApiServices api)
        { 
            var existingAccountIds = new HashSet<long>(db.LivestockFeedAnalysisResults.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockFeedAnalysisResults.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<LivestockFeedAnalysisResultModel, GetLivestockFeedAnalysisResultList>("api/GetLivestockFeedAnalysisResults", new GetLivestockFeedAnalysisResultList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var analysisId= AddNamedParameter(command, "$AnalysisId");
                var parameterId = AddNamedParameter(command, "$ParameterId");
                var asFed = AddNamedParameter(command, "$AsFed");
                var dry = AddNamedParameter(command, "$Dry");

                command.CommandText = $"INSERT or REPLACE INTO LivestockFeedAnalysisResults (Id,Deleted,EntityModifiedOn,ModifiedOn,ModifiedBy,AnalysisId,ParameterId,AsFed,Dry) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                    $"{analysisId.ParameterName},{parameterId.ParameterName},{asFed.ParameterName},{dry.ParameterName})";
                
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    analysisId.Value = model.AnalysisId;
                    parameterId.Value = model.ParameterId;
                    asFed.Value = model.AsFed;
                    dry.Value = model.Dry;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        private async static Task BulkUpdateDuties(FrontEndDbContext db, DbConnection connection, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.Duties.Select(t => t.Id));
            var mostRecentUpdate = db.Duties.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<DutyModel, GetDutyList>("api/GetDuties", new GetDutyList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var name = AddNamedParameter(command, "$Name");
                var daysDue = AddNamedParameter(command, "$DaysDue");
                var dutyType = AddNamedParameter(command, "$DutyType");
                var dutyTypeId = AddNamedParameter(command, "$DutyTypeId");
                var relationship = AddNamedParameter(command, "$Relationship");
                var gender = AddNamedParameter(command, "$Gender");
                var systemRequired = AddNamedParameter(command, "$SystemRequired");
                var livestockTypeId = AddNamedParameter(command, "$LivestockTypeId");

                command.CommandText= $"INSERT or REPLACE INTO LivestockFeedAnalysisResults (Id,Deleted,EntityModifiedOn,ModifiedOn,ModifiedBy,Name,DaysDue,DutyType,DutyTypeId,Relationship,Gender,SystemRequired,LivestockTypeId) " +
                $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                $"{name.ParameterName},{daysDue.ParameterName},{dutyType.ParameterName},{dutyTypeId.ParameterName},{relationship.ParameterName},{gender.ParameterName},{systemRequired.ParameterName},{livestockTypeId.ParameterName})";

                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    name.Value = model.Name;
                    daysDue.Value = model.DaysDue;
                    dutyType.Value = model.DutyType;
                    dutyTypeId.Value = model.DutyTypeId;
                    relationship.Value = model.Relationship;
                    gender.Value = model.Gender ?? string.Empty;
                    systemRequired.Value = model.SystemRequired;
                    livestockTypeId.Value = model.LivestockTypeId.HasValue ? model.LivestockTypeId : DBNull.Value;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        private async static Task BulkUpdateScheduledDuties(FrontEndDbContext db, DbConnection connection, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.ScheduledDuties.Select(t => t.Id));
            var mostRecentUpdate = db.ScheduledDuties.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<ScheduledDutyModel, GetScheduledDutyList>("api/GetScheduledDuties", new GetScheduledDutyList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var dutyId = AddNamedParameter(command, "$DutyId");
                var dismissed = AddNamedParameter(command, "$Dismissed");
                var dueOn = AddNamedParameter(command, "$DueOn");
                var reminderDays = AddNamedParameter(command, "$ReminderDays");
                var completedOn = AddNamedParameter(command, "$CompletedOn");
                var completedBy = AddNamedParameter(command, "$CompletedBy");

                command.CommandText = $"INSERT or REPLACE INTO LivestockFeedAnalysisResults (Id,Deleted,EntityModifiedOn,ModifiedOn,ModifiedBy,DutyId,Dismissed,DueOn,ReminderDays,CompletedOn,CompletedBy) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                    $"{dutyId.ParameterName},{dismissed.ParameterName},{dueOn.ParameterName},{reminderDays.ParameterName},{completedOn.ParameterName},{completedBy.ParameterName})";

                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    dutyId.Value = model.DutyId;
                    dismissed.Value = model.Dismissed;
                    dueOn.Value = model.DueOn;
                    reminderDays.Value = model.ReminderDays;
                    completedOn.Value = model.CompletedOn.HasValue ? model.CompletedOn : DBNull.Value;
                    completedBy.Value = model.CompletedBy.HasValue ? model.CompletedBy : DBNull.Value;
                    await command.ExecuteNonQueryAsync();
                }
               
            }
        }
        private async static Task BulkUpdateMilestones(FrontEndDbContext db, DbConnection connection, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.ScheduledDuties.Select(t => t.Id));
            var mostRecentUpdate = db.Milestones.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<MilestoneModel, GetMilestoneList>("api/GetMilestones", new GetMilestoneList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var subcategory = AddNamedParameter(command, "$Subcategory");
                var systemRequired = AddNamedParameter(command, "$SystemRequired");
                var livestockTypeId = AddNamedParameter(command, "$LivestockTypeId");

                command.CommandText = $"INSERT or REPLACE INTO LivestockFeedAnalysisResults (Id,Deleted,EntityModifiedOn,ModifiedOn,ModifiedBy,Subcategory,SystemRequired,LivestockTypeId) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                    $"{subcategory.ParameterName},{systemRequired.ParameterName},{livestockTypeId.ParameterName})";

                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    subcategory.Value = model.Subcategory;
                    systemRequired.Value = model.SystemRequired;
                    livestockTypeId.Value = model.LivestockTypeId.HasValue ? model.LivestockTypeId : DBNull.Value;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        private async static Task BulkUpdateEvents(FrontEndDbContext db, DbConnection connection, IFrontEndApiServices api)
        {
            var existingAccountIds = new HashSet<long>(db.ScheduledDuties.Select(t => t.Id));
            var mostRecentUpdate = db.Events.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            while (true)
            {
                var returned = await api.ProcessQuery<EventModel, GetEventList>("api/GetEvents", new GetEventList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                if (returned.Item2.Count == 0) break;
                totalCount += returned.Item2.Count;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var name = AddNamedParameter(command, "$Name");
                var color = AddNamedParameter(command, "$Color");
                var startDate = AddNamedParameter(command, "$StartDate");
                var endDate = AddNamedParameter(command, "$EndDate");

                command.CommandText = $"INSERT or REPLACE INTO LivestockFeedAnalysisResults (Id,Deleted,EntityModifiedOn,ModifiedOn,ModifiedBy,Name,Color,StartDate,EndDate) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                    $"{name.ParameterName},{color.ParameterName},{startDate.ParameterName},{endDate.ParameterName})";

                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    name.Value = model.Name;
                    color.Value = model.Color;
                    startDate.Value = model.StartDate;
                    endDate.Value = model.EndDate.HasValue ? model.EndDate : DBNull.Value;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
