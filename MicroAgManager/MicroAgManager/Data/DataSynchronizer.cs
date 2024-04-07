using BackEnd.Abstracts;
using BackEnd.BusinessLogic.FarmLocation;
using BackEnd.BusinessLogic.FarmLocation.LandPlots;
using BackEnd.BusinessLogic.Livestock;
using BackEnd.BusinessLogic.Livestock.Animals;
using BackEnd.BusinessLogic.Livestock.Breeds;
using BackEnd.BusinessLogic.Livestock.Status;
using BackEnd.BusinessLogic.Measure;
using BackEnd.BusinessLogic.Milestone;
using BackEnd.BusinessLogic.Registrar;
using BackEnd.BusinessLogic.Tenant;
using BackEnd.BusinessLogic.Treatment;
using BackEnd.BusinessLogic.Unit;
using BackEnd.Infrastructure;
using Domain.Abstracts;
using Domain.Models;
using Domain.ValueObjects;
using FrontEnd.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Text.Json;
using BackEnd.BusinessLogic.BreedingRecord;
using BackEnd.BusinessLogic.Chore;
using BackEnd.BusinessLogic.Duty;
using BackEnd.BusinessLogic.Event;
using BackEnd.BusinessLogic.ManyToMany;
using BackEnd.BusinessLogic.Measurement;
using BackEnd.BusinessLogic.Registration;
using BackEnd.BusinessLogic.ScheduledDuty;
using BackEnd.BusinessLogic.TreatmentRecord;
using System.Net.Http.Json;
using System.Data.Common;
using Domain.Logic;
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
            }
            _isInitializing = false;
            OnUpdate?.Invoke();
            return await _dbContextFactory.CreateDbContextAsync();
        }
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            PropertyNameCaseInsensitive = true,
            IncludeFields = true
        };
        private static DbParameter AddNamedParameter(DbCommand command, string name)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            command.Parameters.Add(parameter);
            return parameter;
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
        public async Task HandleEntityPushNotification(ModifiedEntityPushNotification notice)
        {
            using (var db = await GetPreparedDbContextAsync())
            {
                Type type = EntityLogic.GetModelType(notice.ModelType);
                var model= BaseModel.ParseJsonString(notice.ModelJson,type) as BaseModel;
                if (model == null) return;
                try { 
                    var local = db.Find(type, model.Id);
                    if (local == null)
                        db.Add(model);
                    else
                    {
                        if (((BaseModel)local).EntityModifiedOn == model.EntityModifiedOn) return;
                        db.Entry(local).CurrentValues.SetValues(model);
                    }
                    await db.SaveChangesAsync();
                    if (HasManyToMany.Contains(type.Name))
                        await SynchManyToMany(db,_api, type);
                    OnUpdate?.Invoke();
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
        }
        private static readonly List<string> HasManyToMany = new List<string>() {nameof(ChoreModel),nameof(EventModel),nameof(MilestoneModel), nameof(DutyModel) };
        private async Task SynchManyToMany(FrontEndDbContext db, IAPIService api, Type modelType)
        {
            db.ChangeTracker.AutoDetectChangesEnabled = false;
            db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            using (var connection = db.Database.GetDbConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    if (modelType is MilestoneModel || modelType is DutyModel)
                        await BulkUpdateDutyMilestone(connection, api);
                    if (modelType is EventModel || modelType is DutyModel)
                        await BulkUpdateDutyEvent(connection, api);
                    if (modelType is ChoreModel || modelType is DutyModel)
                        await BulkUpdateDutyChore(connection, api);
                    transaction.Commit();
                }
            }
        }

        private async static Task BulkUpdateEntities<TModel, TRequest>( FrontEndDbContext db, IAPIService api, string apiEndpoint)
            where TModel : BaseModel, new()
            where TRequest : BaseQuery, new()
        {
            DateTime? dbTime = db.Set<TModel>().OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn.AddTicks(-1) ?? DateTime.MinValue;

  

            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var request = new TRequest()
                {
                    Skip = (int)totalCount,
                    LastModified = dbTime.Value
                };

                var returned = await api.ProcessQuery<TModel, TRequest>(apiEndpoint, request);
                totalCount += returned.Item2.Count;
                expectedCount = returned.Item1;
                Console.WriteLine($"Received {totalCount} of {expectedCount} {typeof(TModel).Name}s from the API");
                if (expectedCount == 0) break;
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.Set<TModel>().FindAsync(model.Id);
                    if (local == null)
                        db.Set<TModel>().Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }
            }
        }
        public async Task EnsureSynchronizingAsync()
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

                    Console.WriteLine("Fetching Data From Server");
                    // Begin fetching any updates to the dataset from the backend server
                    await BulkUpdateEntities<TenantModel, GetTenantList>(db, _api, "api/GetTenants");
                    await Task.WhenAll(
                        BulkUpdateEntities<FarmLocationModel, GetFarmList>(db, _api, "api/GetFarms"),
                        BulkUpdateEntities<LivestockAnimalModel, GetLivestockAnimalList>(db, _api, "api/GetLivestockAnimals"),
                        BulkUpdateEntities<UnitModel, GetUnitList>(db, _api, "api/GetUnits"));
                    await Task.WhenAll(
                        BulkUpdateEntities<MeasureModel, GetMeasureList>(db, _api, "api/GetMeasures"),
                        BulkUpdateEntities<TreatmentModel, GetTreatmentList>(db, _api, "api/GetTreatments"),
                        BulkUpdateEntities<RegistrarModel, GetRegistrarList>(db, _api, "api/GetRegistrars"),
                        BulkUpdateEntities<MilestoneModel, GetMilestoneList>(db, _api, "api/GetMilestones"),
                        BulkUpdateEntities<DutyModel, GetDutyList>(db, _api, "api/GetDuties"),
                        BulkUpdateEntities<EventModel, GetEventList>(db, _api, "api/GetEvents"),
                        BulkUpdateEntities<ChoreModel, GetChoreList>(db, _api, "api/GetChores"));
                    
                    await Task.WhenAll(
                        BulkUpdateEntities<LandPlotModel, GetLandPlotList>(db, _api, "api/GetLandPlots"),
                        BulkUpdateEntities<LivestockBreedModel, GetLivestockBreedList>(db, _api, "api/GetLivestockBreeds"),
                        BulkUpdateEntities<LivestockStatusModel, GetLivestockStatusList>(db, _api, "api/GetLivestockStatuses"),
                        BulkUpdateEntities<ScheduledDutyModel, GetScheduledDutyList>(db, _api, "api/GetScheduledDuties"));

                    await Task.WhenAll(
                        BulkUpdateEntities<LivestockModel, GetLivestockList>(db, _api, "api/GetLivestocks"),
                        BulkUpdateEntities<BreedingRecordModel, GetBreedingRecordList>(db, _api, "api/GetBreedingRecords"),
                        BulkUpdateEntities<RegistrationModel, GetRegistrationList>(db, _api, "api/GetRegistrations"),
                        BulkUpdateEntities<MeasurementModel, GetMeasurementList>(db, _api, "api/GetMeasurements"),
                        BulkUpdateEntities<TreatmentRecordModel, GetTreatmentRecordList>(db, _api, "api/GetTreatmentRecords"));

                    db.ChangeTracker.AutoDetectChangesEnabled = false;
                    db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    using (var connection = db.Database.GetDbConnection())
                    {
                        connection.Open();
                        using (var transaction = connection.BeginTransaction())
                        {
                            await Task.WhenAll(
                                BulkUpdateDutyMilestone(connection, _api),
                                BulkUpdateDutyEvent(connection, _api),
                                BulkUpdateDutyChore(connection, _api));
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
        public async static Task BulkUpdateDutyChore(DbConnection connection, IAPIService api)
        {
            var dataReceived = new List<DutyChore>();
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<DutyChore, GetDutyChoreList>("api/GetDutyChoreList", new GetDutyChoreList { Skip = (int)totalCount });
                expectedCount = returned.Item1;
                totalCount += returned.Item2.Count;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Duty to Chore joins from the API");
                if (expectedCount == 0) break;

                dataReceived.AddRange(returned.Item2);
            }
            var command = connection.CreateCommand();
            var duties = AddNamedParameter(command, "$DutiesId");
            var chores = AddNamedParameter(command, "$ChoresId");

            command.CommandText = $"Delete FROM ChoreModelDutyModel; ";
            await command.ExecuteNonQueryAsync();
            command.CommandText = $"INSERT or REPLACE INTO ChoreModelDutyModel (DutiesId,ChoresId) " +
                $"Values ({duties.ParameterName},{chores.ParameterName})";

            foreach (var model in dataReceived)
            {
                if (model is null) continue;
                duties.Value = model.DutiesId;
                chores.Value = model.ChoresId;
                await command.ExecuteNonQueryAsync();
            }
        }
        public async static Task BulkUpdateDutyEvent(DbConnection connection, IAPIService api)
        {
            var dataReceived = new List<DutyEvent>();
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<DutyEvent, GetDutyEventList>("api/GetDutyEventList", new GetDutyEventList { Skip = (int)totalCount });
                expectedCount = returned.Item1;
                totalCount += returned.Item2.Count;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Duty to Event joins from the API");
                if (expectedCount == 0) break;

                dataReceived.AddRange(returned.Item2);
            }
            var command = connection.CreateCommand();
            var duties = AddNamedParameter(command, "$DutiesId");
            var milestones = AddNamedParameter(command, "$EventsId");

            command.CommandText = $"Delete FROM DutyModelEventModel; ";
            await command.ExecuteNonQueryAsync();
            command.CommandText = $"INSERT or REPLACE INTO DutyModelEventModel (DutiesId,EventsId) " +
                $"Values ({duties.ParameterName},{milestones.ParameterName})";

            foreach (var model in dataReceived)
            {
                if (model is null) continue;
                duties.Value = model.DutiesId;
                milestones.Value = model.EventsId;
                await command.ExecuteNonQueryAsync();
            }
        }
        public async static Task BulkUpdateDutyMilestone(DbConnection connection, IAPIService api)
        {
            var dataReceived = new List<DutyMilestone>();
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<DutyMilestone, GetDutyMilestoneList>("api/GetDutyMilestoneList", new GetDutyMilestoneList { Skip = (int)totalCount });
                expectedCount = returned.Item1;
                totalCount += returned.Item2.Count;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Duty to Milestone joins from the API");
                if (expectedCount == 0) break;
                dataReceived.AddRange(returned.Item2);
            }
            var command = connection.CreateCommand();
            var duties = AddNamedParameter(command, "$DutiesId");
            var milestones = AddNamedParameter(command, "$MilestonesId");

            command.CommandText = $"Delete FROM DutyModelMilestoneModel; ";
            await command.ExecuteNonQueryAsync();
            command.CommandText = $"INSERT or REPLACE INTO DutyModelMilestoneModel (DutiesId,MilestonesId) " +
                $"Values ({duties.ParameterName},{milestones.ParameterName})";

            foreach (var model in dataReceived)
            {
                if (model is null) continue;
                duties.Value = model.DutiesId;
                milestones.Value = model.MilestonesId;
                await command.ExecuteNonQueryAsync();
            }
        }

        public static async Task<Tuple<long, ICollection<T?>>?> ParseTheJSON<T>(HttpResponseMessage result) where T : class
        {
            Console.WriteLine("Parsing the JSON");

            if (typeof(T) == typeof(BreedingRecordModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<BreedingRecordDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(ChoreModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<ChoreDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(DutyModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<DutyDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(EventModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<EventDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(FarmLocationModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<FarmDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(LandPlotModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<LandPlotDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(LivestockAnimalModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<LivestockAnimalDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(LivestockBreedModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<LivestockBreedDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(LivestockStatusModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<LivestockStatusDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(LivestockModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<LivestockDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(MeasureModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<MeasureDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(MeasurementModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<MeasurementDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(MilestoneModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<MilestoneDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(RegistrarModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<RegistrarDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(RegistrationModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<RegistrationDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(ScheduledDutyModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<ScheduledDutyDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(TenantModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<TenantDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(TreatmentModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<TreatmentDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(TreatmentRecordModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<TreatmentRecordDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(UnitModel))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<UnitDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(DutyEvent))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<DutyEventDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(DutyChore))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<DutyChoreDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            if (typeof(T) == typeof(DutyMilestone))
            {
                var returnVal = await result.Content.ReadFromJsonAsync<DutyMilestoneDto>(_jsonOptions);
                return new Tuple<long, ICollection<T?>>(returnVal.Count, returnVal.Models as ICollection<T?>);
            }
            return null;
        }
    }
}
