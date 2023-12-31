using BackEnd.BusinessLogic.BreedingRecord;
using BackEnd.BusinessLogic.Duty;
using BackEnd.BusinessLogic.FarmLocation;
using BackEnd.BusinessLogic.FarmLocation.LandPlots;
using BackEnd.BusinessLogic.Livestock;
using BackEnd.BusinessLogic.Livestock.Animals;
using BackEnd.BusinessLogic.Livestock.Breeds;
using BackEnd.BusinessLogic.Livestock.Status;
using BackEnd.BusinessLogic.ManyToMany;
using BackEnd.BusinessLogic.Measure;
using BackEnd.BusinessLogic.Measurement;
using BackEnd.BusinessLogic.Milestone;
using BackEnd.BusinessLogic.Registrar;
using BackEnd.BusinessLogic.Registration;
using BackEnd.BusinessLogic.ScheduledDuty;
using BackEnd.BusinessLogic.Tenant;
using BackEnd.BusinessLogic.Treatment;
using BackEnd.BusinessLogic.TreatmentRecord;
using BackEnd.BusinessLogic.Unit;
using Domain.Abstracts;
using Domain.Models;
using Domain.ValueObjects;
using System.Data.Common;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using BackEnd.BusinessLogic.Event;
using FrontEnd.Persistence;
using BackEnd.BusinessLogic.Chore;

namespace MicroAgManager.Data
{
    internal static class DomainFetcher
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            PropertyNameCaseInsensitive = true,
            IncludeFields = true
        };
        private static void PopulateBaseModelParameters(Dictionary<string, DbParameter> parameters, BaseModel model)
        {
            parameters["Id"].Value = model.Id;
            parameters["Deleted"].Value = model.Deleted;
            parameters["EntityModifiedOn"].Value = model.EntityModifiedOn;
            parameters["ModifiedBy"].Value = model.ModifiedBy;
        }
        static DbParameter AddNamedParameter(DbCommand command, string name)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            command.Parameters.Add(parameter);
            return parameter;
        }
        private static bool ShouldEntityBeUpdated(List<string>? entityModels, string modelName)=> !(entityModels?.Any() ?? false) || (entityModels?.Contains(modelName) ?? false);
        private static Dictionary<string, DbParameter> GetBaseModelParameters(DbCommand command) =>
        new Dictionary<string, DbParameter>
        {
                {"Id", AddNamedParameter(command, "$Id") },
                {"Deleted", AddNamedParameter(command, "$Deleted") },
                {"EntityModifiedOn",AddNamedParameter(command, "$EntityModifiedOn") },
                {"ModifiedBy",AddNamedParameter(command, "$ModifiedBy") }
        };
        public async static Task BulkUpdateTenants(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(TenantModel))) return;
            var existingAccountIds = new HashSet<Guid>(db.Tenants.Select(t => t.GuidId));
            var mostRecentUpdate = db.Tenants.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<TenantModel, GetTenantList>("api/GetTenants", new GetTenantList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                totalCount += returned.Item2.Count;
                expectedCount= returned.Item1;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Tenants from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var name = AddNamedParameter(command, "$Name");
                var guidId = AddNamedParameter(command, "$GuidId");
                var tenantUserAdminId = AddNamedParameter(command, "$TenantUserAdminId");
                var weatherServiceQueryURL = AddNamedParameter(command, "$WeatherServiceQueryURL");
                command.CommandText = $"INSERT or REPLACE INTO Tenants (Id,GuidId,Name,TenantUserAdminId, Deleted,EntityModifiedOn,ModifiedBy,WeatherServiceQueryURL) " +
                    $"Values ({baseParameters["Id"].ParameterName},{guidId.ParameterName},{name.ParameterName},{tenantUserAdminId.ParameterName},{baseParameters["Deleted"].ParameterName}," +
                    $"{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName},{weatherServiceQueryURL.ParameterName})";
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    name.Value = model.Name;
                    guidId.Value = model.GuidId;
                    tenantUserAdminId.Value = model.TenantUserAdminId;
                    weatherServiceQueryURL.Value =string.IsNullOrEmpty(model.WeatherServiceQueryURL)? DBNull.Value : model.WeatherServiceQueryURL;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async static Task BulkUpdateFarmLocations(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(FarmLocationModel))) return;
            var existingAccountIds = new HashSet<long>(db.Farms.Select(t => t.Id));
            var mostRecentUpdate = db.Farms.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
                long totalCount = 0;
                long expectedCount = 1;
                while (totalCount < expectedCount)
                {
                    var returned = await api.ProcessQuery<FarmLocationModel, GetFarmList>("api/GetFarms", new GetFarmList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                totalCount += returned.Item2.Count;
                    expectedCount = returned.Item1;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Farm Locations from the API");
                if (expectedCount == 0) break;
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

                command.CommandText = $"INSERT or REPLACE INTO Farms (Id,Deleted,EntityModifiedOn,ModifiedBy,TenantId,Name,Longitude,Latitude,StreetAddress,City,State,Zip,Country) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                    $"{tenantId.ParameterName},{name.ParameterName},{longitude.ParameterName},{latitude.ParameterName},{streetAddress.ParameterName},{city.ParameterName},{state.ParameterName},{zipCode.ParameterName},{country.ParameterName})";
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    baseParameters["Id"].Value = model.Id;
                    baseParameters["Deleted"].Value = model.Deleted;
                    baseParameters["EntityModifiedOn"].Value = model.EntityModifiedOn;
                    baseParameters["ModifiedBy"].Value = model.ModifiedBy;
                    tenantId.Value = model.TenantId;
                    name.Value = model.Name;
                    longitude.Value = model.Longitude.HasValue ? model.Longitude.Value : DBNull.Value;
                    latitude.Value = model.Latitude.HasValue ? model.Latitude.Value : DBNull.Value;
                    streetAddress.Value = model.StreetAddress ?? string.Empty;
                    city.Value = model.City ?? string.Empty;
                    state.Value = model.State ?? string.Empty;
                    zipCode.Value = model.Zip ?? string.Empty;
                    country.Value = model.Country ?? string.Empty;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async static Task BulkUpdateLivestockAnimals(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(LivestockAnimalModel))) return;
            var existingAccountIds = new HashSet<long>(db.LivestockAnimals.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockAnimals.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
                long totalCount = 0;
                long expectedCount = 1;
                while (totalCount < expectedCount)
                {
         
                var returned = await api.ProcessQuery<LivestockAnimalModel, GetLivestockAnimalList>("api/GetLivestockAnimals", new GetLivestockAnimalList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
              expectedCount = returned.Item1;
                totalCount += returned.Item2.Count;
                Console.WriteLine($"Received {totalCount} of {expectedCount} LivestockAnimals from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var name = AddNamedParameter(command, "$Name");
                var groupName = AddNamedParameter(command, "$GroupName");
                var parentMaleName = AddNamedParameter(command, "$ParentMaleName");
                var parentFemaleName = AddNamedParameter(command, "$ParentFemaleName");
                var care = AddNamedParameter(command, "$Care");

                command.CommandText = $"INSERT or REPLACE INTO LivestockAnimals (Id,Deleted,EntityModifiedOn,ModifiedBy,Name,GroupName,ParentMaleName,ParentFemaleName,Care) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
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
        public async static Task BulkUpdateLandPlots(List<string>? entityModels,FrontEndDbContext db, DbConnection connection, IAPIService api)
        {

            if (!ShouldEntityBeUpdated(entityModels, nameof(LandPlotModel))) return;

            var existingAccountIds = new HashSet<long>(db.LandPlots.Select(t => t.Id));
            var mostRecentUpdate = db.LandPlots.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<LandPlotModel, GetLandPlotList>("api/GetLandPlots", new GetLandPlotList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                expectedCount = returned.Item1;                  
                totalCount += returned.Item2.Count;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Land Plots from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);

                var farmLocationId = AddNamedParameter(command, "$FarmLocationId");
                var name = AddNamedParameter(command, "$Name");
                var description = AddNamedParameter(command, "$Description");
                var area = AddNamedParameter(command, "$Area");
                var areaUnit = AddNamedParameter(command, "$AreaUnitId");
                var usage = AddNamedParameter(command, "$Usage");
                var parentPlotId = AddNamedParameter(command, "$ParentPlotId");

                command.CommandText = $"INSERT or REPLACE INTO LandPlots (Id,Deleted,EntityModifiedOn,ModifiedBy,FarmLocationId,Name,Description,Area,AreaUnitId,Usage,ParentPlotId) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}" +
                    $",{farmLocationId.ParameterName},{name.ParameterName},{description.ParameterName},{area.ParameterName},{areaUnit.ParameterName},{usage.ParameterName},{parentPlotId.ParameterName})";
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    farmLocationId.Value = model.FarmLocationId;
                    name.Value = model.Name;
                    description.Value = model.Description;
                    area.Value = model.Area;
                    areaUnit.Value = model.AreaUnitId;
                    usage.Value = model.Usage;

                    parentPlotId.Value = model.ParentPlotId.HasValue ? model.ParentPlotId.Value : DBNull.Value;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async static Task BulkUpdateLivestockBreeds(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(LivestockBreedModel))) return;
            var existingAccountIds = new HashSet<long>(db.LivestockBreeds.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockBreeds.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<LivestockBreedModel, GetLivestockBreedList>("api/GetLivestockBreeds", new GetLivestockBreedList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                 
                totalCount += returned.Item2.Count;
                expectedCount = returned.Item1;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Livestock Breeds from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var LivestockAnimalId = AddNamedParameter(command, "$LivestockAnimalId");
                var name = AddNamedParameter(command, "$Name");
                var emojiChar = AddNamedParameter(command, "$EmojiChar");
                var gestationPeriod = AddNamedParameter(command, "$GestationPeriod");
                var heatPeriod = AddNamedParameter(command, "$HeatPeriod");

                command.CommandText = $"INSERT or REPLACE INTO LivestockBreeds (Id,Deleted,EntityModifiedOn,ModifiedBy,LivestockAnimalId,Name,EmojiChar,GestationPeriod,HeatPeriod) " +
                    $"Values ({baseParameters["Id"].ParameterName} , {baseParameters["Deleted"].ParameterName} , {baseParameters["EntityModifiedOn"].ParameterName} , {baseParameters["ModifiedBy"].ParameterName}," +
                    $"{LivestockAnimalId.ParameterName},{name.ParameterName},{emojiChar.ParameterName},{gestationPeriod.ParameterName},{heatPeriod.ParameterName})";
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    LivestockAnimalId.Value = model.LivestockAnimalId;
                    name.Value = model.Name;
                    emojiChar.Value = model.EmojiChar;
                    gestationPeriod.Value = model.GestationPeriod;
                    heatPeriod.Value = model.HeatPeriod;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async static Task BulkUpdateLivestocks(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(LivestockModel)) && !ShouldEntityBeUpdated(entityModels, nameof(BreedingRecordModel))) return;
            var existingAccountIds = new HashSet<long>(db.Livestocks.Select(t => t.Id));
            var mostRecentUpdate = db.Livestocks.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<LivestockModel, GetLivestockList>("api/GetLivestocks", new GetLivestockList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                totalCount += returned.Item2.Count;
                expectedCount = returned.Item1;  
                Console.WriteLine($"Received {totalCount} of {expectedCount} Livestocks from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var motherId = AddNamedParameter(command, "$MotherId");
                var fatherId = AddNamedParameter(command, "$FatherId");
                var statusId = AddNamedParameter(command, "$StatusId");
                var locationId = AddNamedParameter(command, "$LocationId");
                var livestockBreedId = AddNamedParameter(command, "$LivestockBreedId");
                var name = AddNamedParameter(command, "$Name");
                var batchNumber = AddNamedParameter(command, "$BatchNumber");
                var gender = AddNamedParameter(command, "$Gender");
                var variety = AddNamedParameter(command, "$Variety");
                var description = AddNamedParameter(command, "$Description");
                var beingManaged = AddNamedParameter(command, "$BeingManaged");
                var bornDefective = AddNamedParameter(command, "$BornDefective");
                var birthDefect = AddNamedParameter(command, "$BirthDefect");
                var sterile = AddNamedParameter(command, "$Sterile");
                var inMilk = AddNamedParameter(command, "$InMilk");
                var bottleFed = AddNamedParameter(command, "$BottleFed");
                var forSale = AddNamedParameter(command, "$ForSale");
                var birthDate = AddNamedParameter(command, "$Birthdate");



                command.CommandText = $"INSERT or REPLACE INTO Livestocks (Id,Deleted,EntityModifiedOn,ModifiedBy,MotherId,FatherId,LivestockBreedId,Name,BatchNumber,Gender,Variety,Description," +
                    $"BeingManaged,BornDefective,BirthDefect,Sterile,InMilk,BottleFed,ForSale,Birthdate,StatusId,LocationId) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                    $"{motherId.ParameterName},{fatherId.ParameterName},{livestockBreedId.ParameterName},{name.ParameterName},{batchNumber.ParameterName},{gender.ParameterName}," +
                    $"{variety.ParameterName},{description.ParameterName},{beingManaged.ParameterName},{bornDefective.ParameterName},{birthDefect.ParameterName},{sterile.ParameterName}," +
                    $"{inMilk.ParameterName},{bottleFed.ParameterName},{forSale.ParameterName},{birthDate.ParameterName},{statusId.ParameterName},{locationId.ParameterName})";
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    motherId.Value = model.MotherId.HasValue ? model.MotherId.Value : DBNull.Value;
                    fatherId.Value = model.FatherId.HasValue ? model.FatherId.Value : DBNull.Value;
                    locationId.Value = model.LocationId.HasValue ? model.LocationId.Value : DBNull.Value;
                    statusId.Value =  model.StatusId.HasValue ? model.StatusId.Value : DBNull.Value;
                    livestockBreedId.Value = model.LivestockBreedId;
                    name.Value = model.Name;
                    batchNumber.Value = model.BatchNumber;
                    gender.Value = model.Gender;
                    variety.Value = model.Variety;
                    description.Value = model.Description;
                    beingManaged.Value = model.BeingManaged;
                    bornDefective.Value = model.BornDefective;
                    birthDefect.Value = model.BirthDefect;
                    sterile.Value = model.Sterile;
                    inMilk.Value = model.InMilk;
                    bottleFed.Value = model.BottleFed;
                    forSale.Value = model.ForSale;
                    birthDate.Value = model.Birthdate;
                    await command.ExecuteNonQueryAsync();
                }
                
            }
        }
        public async static Task BulkUpdateLivestockStatuses(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(LivestockStatusModel))) return;
            var existingAccountIds = new HashSet<long>(db.LivestockStatuses.Select(t => t.Id));
            var mostRecentUpdate = db.LivestockStatuses.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<LivestockStatusModel, GetLivestockStatusList>("api/GetLivestockStatuses", new GetLivestockStatusList { LastModified = mostRecentUpdate, Skip = (int)totalCount });

                totalCount += returned.Item2.Count;
                expectedCount = returned.Item1;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Livestock Statuses from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var LivestockAnimalId = AddNamedParameter(command, "$LivestockAnimalId");
                var status = AddNamedParameter(command, "$Status");
                var defaultStatus = AddNamedParameter(command, "$DefaultStatus");
                var beingManaged = AddNamedParameter(command, "$BeingManaged");
                var sterile = AddNamedParameter(command, "$Sterile");
                var inMilk = AddNamedParameter(command, "$InMilk");
                var bottleFed = AddNamedParameter(command, "$BottleFed");
                var forSale = AddNamedParameter(command, "$ForSale");

                command.CommandText = $"INSERT or REPLACE INTO LivestockStatuses (Id,Deleted,EntityModifiedOn,ModifiedBy,LivestockAnimalId,Status,DefaultStatus,BeingManaged,Sterile,InMilk,BottleFed,ForSale) " +
                    $"Values ({baseParameters["Id"].ParameterName} , {baseParameters["Deleted"].ParameterName} , {baseParameters["EntityModifiedOn"].ParameterName} , {baseParameters["ModifiedBy"].ParameterName}," +
                    $"{LivestockAnimalId.ParameterName},{status.ParameterName},{defaultStatus.ParameterName},{beingManaged.ParameterName},{sterile.ParameterName},{inMilk.ParameterName},{bottleFed.ParameterName},{forSale.ParameterName})";
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    LivestockAnimalId.Value = model.LivestockAnimalId;
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
        public async static Task BulkUpdateMilestones(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(MilestoneModel))) return;
            var existingAccountIds = new HashSet<long>(db.Milestones.Select(t => t.Id));
            var mostRecentUpdate = db.Milestones.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount< expectedCount)
            {
                var returned = await api.ProcessQuery<MilestoneModel, GetMilestoneList>("api/GetMilestones", new GetMilestoneList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                expectedCount = returned.Item1;
                totalCount += returned.Item2.Count;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Milestones from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var name = AddNamedParameter(command, "$Name");
                var description = AddNamedParameter(command, "$Description");
                var systemRequired = AddNamedParameter(command, "$SystemRequired");
                var recipientTypeId = AddNamedParameter(command, "$RecipientTypeId");
                var recipientType = AddNamedParameter(command, "$RecipientType");

                command.CommandText = $"INSERT or REPLACE INTO Milestones (Id,Deleted,EntityModifiedOn,ModifiedBy,Name,Description,SystemRequired,RecipientTypeId,RecipientType) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                    $"{name.ParameterName},{description.ParameterName},{systemRequired.ParameterName},{recipientTypeId.ParameterName},{recipientType.ParameterName})";

                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    name.Value = model.Name;
                    description.Value = model.Description;
                    systemRequired.Value = model.SystemRequired;
                    recipientTypeId.Value = model.RecipientTypeId;
                    recipientType.Value = model.RecipientType;
                    await command.ExecuteNonQueryAsync();
                }
                
            }
        }
        public async static Task BulkUpdateDuties(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(DutyModel))) return;
            var existingAccountIds = new HashSet<long>(db.Duties.Select(t => t.Id));
            var mostRecentUpdate = db.Duties.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<DutyModel, GetDutyList>("api/GetDuties", new GetDutyList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                expectedCount=returned.Item1;
                totalCount += returned.Item2.Count;
                expectedCount = returned.Item1;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Duties from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var name = AddNamedParameter(command, "$Name");
                var daysDue = AddNamedParameter(command, "$DaysDue");
                var dutyType = AddNamedParameter(command, "$Command");
                var dutyTypeId = AddNamedParameter(command, "$CommandId");
                var relationship = AddNamedParameter(command, "$Relationship");
                var gender = AddNamedParameter(command, "$Gender");
                var systemRequired = AddNamedParameter(command, "$SystemRequired");
                var recipientTypeId = AddNamedParameter(command, "$RecipientTypeId");
                var recipientType = AddNamedParameter(command, "$RecipientType");
                var procedureLink = AddNamedParameter(command, "$ProcedureLink");

                command.CommandText = $"INSERT or REPLACE INTO Duties (Id,Deleted,EntityModifiedOn,ModifiedBy,Name,DaysDue,Command,CommandId,Relationship,Gender,SystemRequired," +
                    $"RecipientTypeId,RecipientType,ProcedureLink) " +
                $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName}," +
                $"{baseParameters["ModifiedBy"].ParameterName}," +
                $"{name.ParameterName},{daysDue.ParameterName},{dutyType.ParameterName},{dutyTypeId.ParameterName},{relationship.ParameterName},{gender.ParameterName}," +
                $"{systemRequired.ParameterName},{recipientTypeId.ParameterName},{recipientType.ParameterName},{procedureLink.ParameterName})";

                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    name.Value = model.Name;
                    daysDue.Value = model.DaysDue;
                    dutyType.Value = model.Command;
                    dutyTypeId.Value = model.CommandId;
                    relationship.Value = model.Relationship;
                    gender.Value = model.Gender ?? string.Empty;
                    procedureLink.Value = model.ProcedureLink ?? string.Empty;
                    systemRequired.Value = model.SystemRequired;
                    recipientTypeId.Value = model.RecipientTypeId;
                    recipientType.Value = model.RecipientType;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async static Task BulkUpdateChores(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(ChoreModel))) return;
            var existingAccountIds = new HashSet<long>(db.Chores.Select(t => t.Id));
            var mostRecentUpdate = db.Duties.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<ChoreModel, GetChoreList>("api/GetChores", new GetChoreList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                expectedCount = returned.Item1;
                totalCount += returned.Item2.Count;
                expectedCount = returned.Item1;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Chores from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var recipientTypeId = AddNamedParameter(command, "$RecipientTypeId");
                var recipientType = AddNamedParameter(command, "$RecipientType");
                var name = AddNamedParameter(command, "$Name");
                var color = AddNamedParameter(command, "$Color");
                var dueByTime = AddNamedParameter(command, "$DueByTime");
                
                var frequency = AddNamedParameter(command, "$Frequency");
                var frequencyUnitId = AddNamedParameter(command, "$FrequencyUnitId");
                var period = AddNamedParameter(command, "$Period");
                var periodUnitId = AddNamedParameter(command, "$PeriodUnitId");


                command.CommandText = $"INSERT or REPLACE INTO Chores (Id,Deleted,EntityModifiedOn,ModifiedBy,Name,RecipientTypeId,RecipientType,"
                    + "Color,DueByTime,Frequency,FrequencyUnitId,Period,PeriodUnitId ) " +
                $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName}," +
                $"{baseParameters["ModifiedBy"].ParameterName}," +
                $"{name.ParameterName},{recipientTypeId.ParameterName},{recipientType.ParameterName},{color.ParameterName},{dueByTime.ParameterName},{frequency.ParameterName},"+
                $"{frequencyUnitId.ParameterName},{period.ParameterName},{periodUnitId.ParameterName})";

                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    name.Value = model.Name;
                    recipientTypeId.Value = model.RecipientTypeId;
                    recipientType.Value = model.RecipientType;
                    color.Value = model.Color;
                    dueByTime.Value = model.DueByTime;
                    frequency.Value = model.Frequency;
                    frequencyUnitId.Value = model.FrequencyUnitId;
                    period.Value = model.PeriodUnitId;
                    periodUnitId.Value = model.PeriodUnitId;
                   
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async static Task BulkUpdateBreedingRecords(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(BreedingRecordModel))) return;
            var existingAccountIds = new HashSet<long>(db.BreedingRecords.Select(t => t.Id));
            var mostRecentUpdate = db.BreedingRecords.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<BreedingRecordModel, GetBreedingRecordList>("api/GetBreedingRecords", new GetBreedingRecordList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                expectedCount=returned.Item1;
                totalCount += returned.Item2.Count;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Breeding Records from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var femaleId = AddNamedParameter(command, "$FemaleId");
                var maleId = AddNamedParameter(command, "$MaleId");
                var notes = AddNamedParameter(command, "$Notes");
                var serviceDate = AddNamedParameter(command, "$ServiceDate");
                var resolutionDate = AddNamedParameter(command, "$ResolutionDate");
                var stillBornMales = AddNamedParameter(command, "$StillBornMales");
                var stillBornFemales = AddNamedParameter(command, "$StillBornFemales");
                var bornMales = AddNamedParameter(command, "$BornMales");
                var bornFemales = AddNamedParameter(command, "$BornFemales");
                var resolution = AddNamedParameter(command, "$Reolution");

                command.CommandText = $"INSERT or REPLACE INTO BreedingRecords (Id,Deleted,EntityModifiedOn,ModifiedBy,FemaleId,MaleId,ServiceDate,ResolutionDate,StillBornMales,StillBornFemales,Notes,BornMales,BornFemales,Resolution) " +
                $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                $"{femaleId.ParameterName},{maleId.ParameterName},{serviceDate.ParameterName},{resolutionDate.ParameterName},{stillBornMales.ParameterName},{stillBornFemales.ParameterName},{notes.ParameterName},{bornMales.ParameterName},{bornFemales.ParameterName},{resolution.ParameterName})";

                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    serviceDate.Value = model.ServiceDate;
                    resolutionDate.Value = model.ResolutionDate.HasValue ? model.ResolutionDate : DBNull.Value;
                    femaleId.Value = model.FemaleId;
                    maleId.Value = model.MaleId.HasValue ? model.MaleId : DBNull.Value;
                    stillBornMales.Value= model.StillbornMales.HasValue ? model.StillbornMales : DBNull.Value;
                    stillBornFemales.Value = model.StillbornFemales.HasValue ? model.StillbornFemales : DBNull.Value;
                    bornFemales.Value = model.BornFemales.HasValue ? model.BornFemales : DBNull.Value;
                    bornMales.Value = model.BornMales.HasValue ? model.BornMales : DBNull.Value;
                    resolution.Value = model.Resolution ?? string.Empty;
                    notes.Value = model.Notes ?? string.Empty;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async static Task BulkUpdateScheduledDuties(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(ScheduledDutyModel))) return;
            var existingAccountIds = new HashSet<long>(db.ScheduledDuties.Select(t => t.Id));
            var mostRecentUpdate = db.ScheduledDuties.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<ScheduledDutyModel, GetScheduledDutyList>("api/GetScheduledDuties", new GetScheduledDutyList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                expectedCount=returned.Item1;
                totalCount += returned.Item2.Count;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Scheduled Duties from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var dutyId = AddNamedParameter(command, "$DutyId");
                var dismissed = AddNamedParameter(command, "$Dismissed");
                var dueOn = AddNamedParameter(command, "$DueOn");
                var reminderDays = AddNamedParameter(command, "$ReminderDays");
                var completedOn = AddNamedParameter(command, "$CompletedOn");
                var completedBy = AddNamedParameter(command, "$CompletedBy");
                var recordId = AddNamedParameter(command, "$RecordId");
                var record = AddNamedParameter(command, "$Record");
                var recipientId = AddNamedParameter(command, "$RecipientId");
                var recipient = AddNamedParameter(command, "$Recipient");

                command.CommandText = $"INSERT or REPLACE INTO ScheduledDuties (Id,Deleted,EntityModifiedOn,ModifiedBy,DutyId,Dismissed,DueOn,ReminderDays,CompletedOn,CompletedBy,RecordId,Record," +
                    $"RecipientId,Recipient) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                    $"{dutyId.ParameterName},{dismissed.ParameterName},{dueOn.ParameterName},{reminderDays.ParameterName},{completedOn.ParameterName},{completedBy.ParameterName},{recordId.ParameterName}," +
                    $"{record.ParameterName},{recipientId.ParameterName},{recipient.ParameterName})";

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
                    recordId.Value = model.RecordId.HasValue ? model.RecordId : DBNull.Value;
                    record.Value = model.Record;
                    recipientId.Value = model.RecipientId;
                    recipient.Value = model.Recipient;
                    await command.ExecuteNonQueryAsync();
                }

            }
        }
        public async static Task BulkUpdateRegistrars(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(RegistrarModel))) return;
            var existingAccountIds = new HashSet<long>(db.Registrars.Select(t => t.Id));
            var mostRecentUpdate = db.Registrars.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<RegistrarModel, GetRegistrarList>("api/GetRegistrars", new GetRegistrarList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                expectedCount=returned.Item1;
                totalCount += returned.Item2.Count;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Registrars from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var name = AddNamedParameter(command, "$Name");
                var email = AddNamedParameter(command, "$Email");
                var website = AddNamedParameter(command, "$Website");
                var registrarApi = AddNamedParameter(command, "$API");
                var registrarFarmID = AddNamedParameter(command, "$RegistrarFarmID");

                command.CommandText = $"INSERT or REPLACE INTO Registrars (Id,Deleted,EntityModifiedOn,ModifiedBy,Name,Email,Website,API,RegistrarFarmID) " +
                $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                $"{name.ParameterName},{email.ParameterName},{website.ParameterName},{registrarApi.ParameterName},{registrarFarmID.ParameterName})";

                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    name.Value = model.Name;
                    email.Value=model.Email ?? string.Empty;
                    website.Value = model.Website ?? string.Empty;
                    registrarApi.Value = model.API ?? string.Empty;
                    registrarFarmID.Value = model.RegistrarFarmID ?? string.Empty;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async static Task BulkUpdateRegistrations(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(RegistrationModel))) return;
            var existingAccountIds = new HashSet<long>(db.Registrations.Select(t => t.Id));
            var mostRecentUpdate = db.Registrations.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<RegistrationModel, GetRegistrationList>("api/GetRegistrations", new GetRegistrationList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                expectedCount=returned.Item1;
                totalCount += returned.Item2.Count;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Registrations from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var registarId= AddNamedParameter(command, "$RegistrarId");
                var recipientTypeId = AddNamedParameter(command, "$RecipientTypeId");
                var recipientId = AddNamedParameter(command, "$RecipientId");
                var recipientType = AddNamedParameter(command, "$RecipientType");
                var registrationDate = AddNamedParameter(command, "$RegistrationDate");
                var identifier = AddNamedParameter(command, "$Identifier");
                var defaultIdentification = AddNamedParameter(command, "$DefaultIdentification");

                command.CommandText = $"INSERT or REPLACE INTO Registrations (Id,Deleted,EntityModifiedOn,ModifiedBy,RegistrarId,RecipientTypeId,RecipientId,RecipientType,RegistrationDate,Identifier,DefaultIdentification) " +
                    $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                    $"{registarId.ParameterName},{recipientTypeId.ParameterName},{recipientId.ParameterName},{recipientType.ParameterName},{registrationDate.ParameterName},{identifier.ParameterName},{defaultIdentification.ParameterName})";

                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    registarId.Value = model.RegistrarId;
                    recipientTypeId.Value = model.RecipientTypeId;
                    recipientId.Value = model.RecipientId;
                    recipientType.Value = model.RecipientType;
                    registrationDate.Value = model.RegistrationDate;
                    identifier.Value = model.Identifier;
                    defaultIdentification.Value = model.DefaultIdentification;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        //public async static Task BulkUpdateLivestockFeeds(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        //{
        //    if (!ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedModel))) return;
        //    var existingAccountIds = new HashSet<long>(db.LivestockFeeds.Select(t => t.Id));
        //    var mostRecentUpdate = db.LivestockFeeds.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
        //    long totalCount = 0;
        //    while (true)
        //    {
        //        var returned = await api.ProcessQuery<LivestockFeedModel, GetLivestockFeedList>("api/GetLivestockFeeds", new GetLivestockFeedList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
        //        expectedCount=returned.Item1;
        //        totalCount += returned.Item2.Count;
        //        var command = connection.CreateCommand();
        //        var baseParameters = GetBaseModelParameters(command);
        //        var LivestockAnimalId = AddNamedParameter(command, "$LivestockAnimalId");
        //        var name = AddNamedParameter(command, "$Name");
        //        var source = AddNamedParameter(command, "$Source");
        //        var cutting = AddNamedParameter(command, "$Cutting");
        //        var active = AddNamedParameter(command, "$Active");
        //        var quantity = AddNamedParameter(command, "$Quantity");
        //        var quantityUnit = AddNamedParameter(command, "$QuantityUnit");
        //        var quantityWarning = AddNamedParameter(command, "$QuantityWarning");
        //        var feedType = AddNamedParameter(command, "$FeedType");
        //        var distribution = AddNamedParameter(command, "$Distribution");

        //        command.CommandText = $"INSERT or REPLACE INTO LivestockFeeds (Id,Deleted,EntityModifiedOn,ModifiedBy,LivestockAnimalId,Name,Source,Cutting,Active,Quantity,QuantityUnit,QuantityWarning,FeedType,Distribution) " +
        //            $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
        //            $"{LivestockAnimalId.ParameterName},{name.ParameterName},{source.ParameterName},{cutting.ParameterName},{active.ParameterName},{quantity.ParameterName},{quantityUnit.ParameterName},{quantityWarning.ParameterName},{feedType.ParameterName},{distribution.ParameterName})";
        //        foreach (var model in returned.Item2)
        //        {
        //            if (model is null) continue;
        //            PopulateBaseModelParameters(baseParameters, model);
        //            LivestockAnimalId.Value = model.LivestockAnimalId;
        //            name.Value = model.Name;
        //            source.Value = model.Source;

        //            cutting.Value = model.Cutting.HasValue ? model.Cutting : DBNull.Value;
        //            active.Value = model.Active;
        //            quantity.Value = model.Quantity;
        //            quantityUnit.Value = model.QuantityUnit;
        //            quantityWarning.Value = model.QuantityWarning;
        //            feedType.Value = model.FeedType;
        //            distribution.Value = model.Distribution;
        //            await command.ExecuteNonQueryAsync();
        //        }
        //    }
        //}
        //public async static Task BulkUpdateLivestockFeedServings(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        //{
        //    if (!ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedServingModel))) return;
        //    var existingAccountIds = new HashSet<long>(db.LivestockFeedServings.Select(t => t.Id));
        //    var mostRecentUpdate = db.LivestockFeedServings.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
        //    long totalCount = 0;
        //    while (true)
        //    {
        //        var returned = await api.ProcessQuery<LivestockFeedServingModel, GetLivestockFeedServingList>("api/GetLivestockFeedServings", new GetLivestockFeedServingList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
        //        expectedCount=returned.Item1;
        //        totalCount += returned.Item2.Count;
        //        var command = connection.CreateCommand();
        //        var baseParameters = GetBaseModelParameters(command);
        //        var feedId = AddNamedParameter(command, "$FeedId");
        //        var statusId = AddNamedParameter(command, "$StatusId");
        //        var servingFrequency = AddNamedParameter(command, "$ServingFrequency");
        //        var serving = AddNamedParameter(command, "$Serving");

        //        command.CommandText = $"INSERT or REPLACE INTO LivestockFeedServings (Id,Deleted,EntityModifiedOn,ModifiedBy,FeedId,StatusId,ServingFrequency,Serving) " +
        //            $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
        //            $"{feedId.ParameterName},{statusId.ParameterName},{servingFrequency.ParameterName},{serving.ParameterName})";
        //        foreach (var model in returned.Item2)
        //        {
        //            if (model is null) continue;
        //            PopulateBaseModelParameters(baseParameters, model);
        //            feedId.Value = model.FeedId;
        //            statusId.Value = model.StatusId;
        //            servingFrequency.Value = model.ServingFrequency;
        //            serving.Value = model.Serving;
        //            await command.ExecuteNonQueryAsync();
        //        }
        //    }
        //}
        //public async static Task BulkUpdateLivestockFeedDistributions(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        //{
        //    if (!ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedDistributionModel))) return;
        //    var existingAccountIds = new HashSet<long>(db.LivestockFeedDistributions.Select(t => t.Id));
        //    var mostRecentUpdate = db.LivestockFeedDistributions.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
        //    long totalCount = 0;
        //    while (true)
        //    {
        //        var returned = await api.ProcessQuery<LivestockFeedDistributionModel, GetLivestockFeedDistributionList>("api/GetLivestockFeedDistributions", new GetLivestockFeedDistributionList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
        //        expectedCount=returned.Item1;
        //        totalCount += returned.Item2.Count;
        //        var command = connection.CreateCommand();
        //        var baseParameters = GetBaseModelParameters(command);
        //        var feedId = AddNamedParameter(command, "$FeedId");
        //        var quantity = AddNamedParameter(command, "$Quantity");
        //        var discarded = AddNamedParameter(command, "$Discarded");
        //        var note = AddNamedParameter(command, "$Note");
        //        var datePerformed = AddNamedParameter(command, "$DatePerformed");

        //        command.CommandText = $"INSERT or REPLACE INTO LivestockFeedDistributions (Id,Deleted,EntityModifiedOn,ModifiedBy,FeedId,Quantity,Discarded,Note,DatePerformed) " +
        //            $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
        //            $"{feedId.ParameterName},{quantity.ParameterName},{discarded.ParameterName},{note.ParameterName},{datePerformed.ParameterName})";
        //        foreach (var model in returned.Item2)
        //        {
        //            if (model is null) continue;
        //            PopulateBaseModelParameters(baseParameters, model);
        //            feedId.Value = model.FeedId;
        //            quantity.Value = model.Quantity;
        //            discarded.Value = model.Discarded.HasValue ? model.Discarded : DBNull.Value;
        //            note.Value = model.Note;
        //            datePerformed.Value = model.DatePerformed;
        //            await command.ExecuteNonQueryAsync();
        //        }
        //    }
        //}
        //public async static Task BulkUpdateLivestockFeedAnalysisParameters(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        //{
        //    if (!ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedAnalysisParameterModel))) return;
        //    var existingAccountIds = new HashSet<long>(db.LivestockFeedAnalysisParameters.Select(t => t.Id));
        //    var mostRecentUpdate = db.LivestockFeedAnalysisParameters.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
        //    long totalCount = 0;
        //    while (true)
        //    {
        //        var returned = await api.ProcessQuery<LivestockFeedAnalysisParameterModel, GetLivestockFeedAnalysisParameterList>("api/GetLivestockFeedAnalysisParameters", new GetLivestockFeedAnalysisParameterList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
        //        expectedCount=returned.Item1;
        //        totalCount += returned.Item2.Count;
        //        var command = connection.CreateCommand();
        //        var baseParameters = GetBaseModelParameters(command);
        //        var parameter = AddNamedParameter(command, "$Parameter");
        //        var subParameter = AddNamedParameter(command, "$SubParameter");
        //        var unit = AddNamedParameter(command, "$Unit");
        //        var method = AddNamedParameter(command, "$Method");
        //        var reportOrder = AddNamedParameter(command, "$ReportOrder");

        //        command.CommandText = $"INSERT or REPLACE INTO LivestockFeedAnalysisParameters (Id,Deleted,EntityModifiedOn,ModifiedBy,Parameter,SubParameter,Unit,Method,ReportOrder) " +
        //            $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
        //            $"{parameter.ParameterName},{subParameter.ParameterName},{unit.ParameterName},{method.ParameterName},{reportOrder.ParameterName})";
        //        foreach (var model in returned.Item2)
        //        {
        //            if (model is null) continue;
        //            PopulateBaseModelParameters(baseParameters, model);
        //            parameter.Value = model.Parameter;
        //            subParameter.Value = model.SubParameter;
        //            unit.Value = model.Unit;
        //            method.Value = model.Method;
        //            reportOrder.Value = model.ReportOrder;
        //            await command.ExecuteNonQueryAsync();
        //        }

        //    }
        //}
        //public async static Task BulkUpdateLivestockFeedAnalyses(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        //{
        //    if (!ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedAnalysisModel))) return;
        //    var existingAccountIds = new HashSet<long>(db.LivestockFeedAnalyses.Select(t => t.Id));
        //    var mostRecentUpdate = db.LivestockFeedAnalyses.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
        //    long totalCount = 0;
        //    while (true)
        //    {
        //        var returned = await api.ProcessQuery<LivestockFeedAnalysisModel, GetLivestockFeedAnalysisList>("api/GetLivestockFeedAnalyses", new GetLivestockFeedAnalysisList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
        //        expectedCount=returned.Item1;
        //        totalCount += returned.Item2.Count;
        //        var command = connection.CreateCommand();
        //        var baseParameters = GetBaseModelParameters(command);
        //        var feedId = AddNamedParameter(command, "$FeedId");
        //        var labNumber = AddNamedParameter(command, "$LabNumber");
        //        var testCode = AddNamedParameter(command, "$TestCode");
        //        var dateSampled = AddNamedParameter(command, "$DateSampled");
        //        var dateReceived = AddNamedParameter(command, "$DateReceived");
        //        var dateReported = AddNamedParameter(command, "$DateReported");
        //        var datePrinted = AddNamedParameter(command, "$DatePrinted");

        //        command.CommandText = $"INSERT or REPLACE INTO LivestockFeedAnalyses (Id,Deleted,EntityModifiedOn,ModifiedBy,FeedId,LabNumber,TestCode,DateSampled,DateReceived,DateReported,DatePrinted) " +
        //            $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
        //            $"{feedId.ParameterName},{labNumber.ParameterName},{testCode.ParameterName},{dateSampled.ParameterName},{dateReceived.ParameterName},{dateReported.ParameterName},{datePrinted.ParameterName})";

        //        foreach (var model in returned.Item2)
        //        {
        //            if (model is null) continue;
        //            PopulateBaseModelParameters(baseParameters, model);
        //            feedId.Value = model.FeedId;
        //            labNumber.Value = model.LabNumber;
        //            testCode.Value = model.TestCode;
        //            dateSampled.Value = model.DateSampled.HasValue ? model.DateSampled : DBNull.Value;
        //            dateReceived.Value = model.DateReceived.HasValue ? model.DateReceived : DBNull.Value;
        //            dateReported.Value = model.DateReported.HasValue ? model.DateReported : DBNull.Value;
        //            datePrinted.Value = model.DatePrinted.HasValue ? model.DatePrinted : DBNull.Value;
        //            await command.ExecuteNonQueryAsync();
        //        }
        //    }
        //}
        //public async static Task BulkUpdateLivestockFeedAnalysisResults(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        //{
        //    if (!ShouldEntityBeUpdated(entityModels, nameof(LivestockFeedAnalysisResultModel))) return;

        //    var existingAccountIds = new HashSet<long>(db.LivestockFeedAnalysisResults.Select(t => t.Id));
        //    var mostRecentUpdate = db.LivestockFeedAnalysisResults.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
        //    long totalCount = 0;
        //    while (true)
        //    {
        //        var returned = await api.ProcessQuery<LivestockFeedAnalysisResultModel, GetLivestockFeedAnalysisResultList>("api/GetLivestockFeedAnalysisResults", new GetLivestockFeedAnalysisResultList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
        //        expectedCount=returned.Item1;
        //        totalCount += returned.Item2.Count;
        //        var command = connection.CreateCommand();
        //        var baseParameters = GetBaseModelParameters(command);
        //        var analysisId = AddNamedParameter(command, "$AnalysisId");
        //        var parameterId = AddNamedParameter(command, "$ParameterId");
        //        var asFed = AddNamedParameter(command, "$AsFed");
        //        var dry = AddNamedParameter(command, "$Dry");

        //        command.CommandText = $"INSERT or REPLACE INTO LivestockFeedAnalysisResults (Id,Deleted,EntityModifiedOn,ModifiedBy,AnalysisId,ParameterId,AsFed,Dry) " +
        //            $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
        //            $"{analysisId.ParameterName},{parameterId.ParameterName},{asFed.ParameterName},{dry.ParameterName})";

        //        foreach (var model in returned.Item2)
        //        {
        //            if (model is null) continue;
        //            PopulateBaseModelParameters(baseParameters, model);
        //            analysisId.Value = model.AnalysisId;
        //            parameterId.Value = model.ParameterId;
        //            asFed.Value = model.AsFed;
        //            dry.Value = model.Dry;
        //            await command.ExecuteNonQueryAsync();
        //        }
        //    }
        //}

        //public async static Task BulkUpdateEvents(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        //{
        //    if (!ShouldEntityBeUpdated(entityModels, nameof(EventModel))) return;
        //    var existingAccountIds = new HashSet<long>(db.ScheduledDuties.Select(t => t.Id));
        //    var mostRecentUpdate = db.Events.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
        //    long totalCount = 0;
        //    while (true)
        //    {
        //        var returned = await api.ProcessQuery<EventModel, GetEventList>("api/GetEvents", new GetEventList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
        //        expectedCount=returned.Item1;
        //        totalCount += returned.Item2.Count;
        //        var command = connection.CreateCommand();
        //        var baseParameters = GetBaseModelParameters(command);
        //        var name = AddNamedParameter(command, "$Name");
        //        var color = AddNamedParameter(command, "$Color");
        //        var startDate = AddNamedParameter(command, "$StartDate");
        //        var endDate = AddNamedParameter(command, "$EndDate");

        //        command.CommandText = $"INSERT or REPLACE INTO LivestockFeedAnalysisResults (Id,Deleted,EntityModifiedOn,ModifiedBy,Name,Color,StartDate,EndDate) " +
        //            $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
        //            $"{name.ParameterName},{color.ParameterName},{startDate.ParameterName},{endDate.ParameterName})";

        //        foreach (var model in returned.Item2)
        //        {
        //            if (model is null) continue;
        //            PopulateBaseModelParameters(baseParameters, model);
        //            name.Value = model.Name;
        //            color.Value = model.Color;
        //            startDate.Value = model.StartDate;
        //            endDate.Value = model.EndDate.HasValue ? model.EndDate : DBNull.Value;
        //            await command.ExecuteNonQueryAsync();
        //        }
        //    }
        //}

        public async static Task BulkUpdateUnits(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(UnitModel))) return;
            var existingAccountIds = new HashSet<long>(db.Units.Select(t => t.Id));
            var mostRecentUpdate = db.Units.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<UnitModel, GetUnitList>("api/GetUnits", new GetUnitList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                expectedCount=returned.Item1;
                totalCount += returned.Item2.Count;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Units from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var name = AddNamedParameter(command, "$Name");
                var category = AddNamedParameter(command, "$Category");
                var symbol = AddNamedParameter(command, "$Symbol");
                var conversionFactorToSIUnit = AddNamedParameter(command, "$ConversionFactorToSIUnit");

                command.CommandText = $"INSERT or REPLACE INTO Units (Id,Deleted,EntityModifiedOn,ModifiedBy,Name,Category,Symbol,ConversionFactorToSIUnit) " +
                $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                $"{name.ParameterName},{category.ParameterName},{symbol.ParameterName},{conversionFactorToSIUnit.ParameterName})";

                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    name.Value = model.Name;
                    category.Value = model.Category;
                    symbol.Value = model.Symbol;
                    conversionFactorToSIUnit.Value = model.ConversionFactorToSIUnit;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async static Task BulkUpdateMeasures(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(MeasureModel))) return;
            var existingAccountIds = new HashSet<long>(db.Measures.Select(t => t.Id));
            var mostRecentUpdate = db.Measures.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<MeasureModel, GetMeasureList>("api/GetMeasures", new GetMeasureList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                expectedCount=returned.Item1;
                totalCount += returned.Item2.Count;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Measures from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var name = AddNamedParameter(command, "$Name");
                var method = AddNamedParameter(command, "$Method");
                var unitId = AddNamedParameter(command, "$UnitId");

                command.CommandText = $"INSERT or REPLACE INTO Measures (Id,Deleted,EntityModifiedOn,ModifiedBy,Name,Method,UnitId) " +
                $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                $"{name.ParameterName},{method.ParameterName},{unitId.ParameterName})";

                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    name.Value = model.Name;
                    method.Value = model.Method;
                    unitId.Value = model.UnitId;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async static Task BulkUpdateMeasurements(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(MeasurementModel))) return;
            var existingAccountIds = new HashSet<long>(db.Measurements.Select(t => t.Id));
            var mostRecentUpdate = db.Measurements.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<MeasurementModel, GetMeasurementList>("api/GetMeasurements", new GetMeasurementList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                expectedCount=returned.Item1;
                totalCount += returned.Item2.Count;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Measurements from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var measureId = AddNamedParameter(command, "$MeasureId");
                var recipientTypeId = AddNamedParameter(command, "$RecipientTypeId");
                var recipientType = AddNamedParameter(command, "$RecipientType");
                var recipientId = AddNamedParameter(command, "$RecipientId");
                var value = AddNamedParameter(command, "$Value");
                var measurementUnitId = AddNamedParameter(command, "$MeasurementUnitId");
                var notes = AddNamedParameter(command, "$Notes");
                var datePerformed = AddNamedParameter(command, "$DatePerformed");


                command.CommandText = $"INSERT or REPLACE INTO Measurements (Id,Deleted,EntityModifiedOn,ModifiedBy,MeasureId,RecipientTypeId,RecipientType,RecipientId,Value,MeasurementUnitId,Notes,DatePerformed) " +
                $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                $"{measureId.ParameterName},{recipientTypeId.ParameterName},{recipientType.ParameterName},{recipientId.ParameterName}," +
                $"{value.ParameterName},{measurementUnitId.ParameterName},{notes.ParameterName},{datePerformed.ParameterName})";

                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    measureId.Value = model.MeasureId;
                    recipientTypeId.Value = model.RecipientTypeId;
                    recipientType.Value = model.RecipientType;
                    recipientId.Value = model.RecipientId;
                    value.Value = model.Value;
                    measurementUnitId.Value = model.MeasurementUnitId;
                    notes.Value = model.Notes ?? string.Empty;
                    datePerformed.Value = model.DatePerformed;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async static Task BulkUpdateTreatments(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {

            if (!ShouldEntityBeUpdated(entityModels, nameof(TreatmentModel))) return;
            var existingAccountIds = new HashSet<long>(db.Treatments.Select(t => t.Id));
            var mostRecentUpdate = db.Treatments.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<TreatmentModel, GetTreatmentList>("api/GetTreatments", new GetTreatmentList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                expectedCount=returned.Item1;
                totalCount += returned.Item2.Count;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Treatments from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var name = AddNamedParameter(command, "$Name");
                var brandBame = AddNamedParameter(command, "$BrandName");
                var reason = AddNamedParameter(command, "$Reason");
                var labelMethod = AddNamedParameter(command, "$LabelMethod");
                var meatWithdrawal = AddNamedParameter(command, "$MeatWithdrawal");
                var milkWithdrawal = AddNamedParameter(command, "$MilkWithdrawal");
                var dosageAmount = AddNamedParameter(command, "$DosageAmount");
                var dosageUnitId = AddNamedParameter(command, "$DosageUnitId");
                var animalMass = AddNamedParameter(command, "$RecipientMass");
                var animalMassUnitId = AddNamedParameter(command, "$RecipientMassUnitId");
                var frequency = AddNamedParameter(command, "$Frequency");
                var frequencyUnitId = AddNamedParameter(command, "$FrequencyUnitId");
                var duration = AddNamedParameter(command, "$Duration");
                var durationUnitId = AddNamedParameter(command, "$DurationUnitId");


                command.CommandText = $"INSERT or REPLACE INTO Treatments (Id,Deleted,EntityModifiedOn,ModifiedBy,[Name],[BrandName],[Reason],[LabelMethod],[MeatWithdrawal]" +
                    $",[MilkWithdrawal],[DosageAmount],[DosageUnitId],[RecipientMass],[RecipientMassUnitId],[Frequency],[FrequencyUnitId],[Duration],[DurationUnitId]) " +
                $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                $"{name.ParameterName},{brandBame.ParameterName},{reason.ParameterName},{labelMethod.ParameterName},{meatWithdrawal.ParameterName},{milkWithdrawal.ParameterName}" +
                $",{dosageAmount.ParameterName},{dosageUnitId.ParameterName},{animalMass.ParameterName},{animalMassUnitId.ParameterName},{frequency.ParameterName},{frequencyUnitId.ParameterName}" +
                $",{duration.ParameterName},{durationUnitId.ParameterName})";

                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    name.Value = model.Name;
                    brandBame.Value = model.BrandName;
                    reason.Value = model.Reason;
                    labelMethod.Value = model.LabelMethod;
                    meatWithdrawal.Value = model.MeatWithdrawal;
                    milkWithdrawal.Value = model.MilkWithdrawal;
                    dosageAmount.Value = model.DosageAmount;
                    dosageUnitId.Value = model.DosageUnitId.HasValue ? model.DosageUnitId : DBNull.Value;
                    animalMass.Value = model.RecipientMass;
                    animalMassUnitId.Value = model.RecipientMassUnitId.HasValue ? model.RecipientMassUnitId : DBNull.Value;
                    frequency.Value = model.Frequency;
                    frequencyUnitId.Value = model.FrequencyUnitId.HasValue ? model.FrequencyUnitId:DBNull.Value;
                    duration.Value = model.Duration;
                    durationUnitId.Value = model.DurationUnitId.HasValue ? model.DosageUnitId:DBNull.Value;
                    await command.ExecuteNonQueryAsync();
                }
            }

        }
        public async static Task BulkUpdateTreatmentRecords(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels,nameof(TreatmentRecordModel))) return;
            var existingAccountIds = new HashSet<long>(db.TreatmentRecords.Select(t => t.Id));
            var mostRecentUpdate = db.TreatmentRecords.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<TreatmentRecordModel, GetTreatmentRecordList>("api/GetTreatmentRecords", new GetTreatmentRecordList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                expectedCount=returned.Item1;
                totalCount += returned.Item2.Count;
                Console.WriteLine($"Received {totalCount} of {expectedCount} TreatmentRecords from the API");
                if (expectedCount == 0) break;
                var command = connection.CreateCommand();
                var baseParameters = GetBaseModelParameters(command);
                var treatmentId = AddNamedParameter(command, "$TreatmentId");
                var recipientTypeId = AddNamedParameter(command, "$RecipientTypeId");
                var recipientType = AddNamedParameter(command, "$RecipientType");
                var recipientId = AddNamedParameter(command, "$RecipientId");
                var notes = AddNamedParameter(command, "$Notes");
                var datePerformed = AddNamedParameter(command, "$DatePerformed");
                var dosageAmount = AddNamedParameter(command,"$DosageAmount");
                var dosageUnitId = AddNamedParameter(command,"$DosageUnitId");
                var appliedMethod = AddNamedParameter(command,"$AppliedMethod");
                

                command.CommandText = $"INSERT or REPLACE INTO TreatmentRecords (Id,Deleted,EntityModifiedOn,ModifiedBy," +
                    $"[TreatmentId],[RecipientTypeId],[RecipientType],[RecipientId],[Notes],[DatePerformed],[DosageAmount],[DosageUnitId],[AppliedMethod]) " +
               $"Values ({baseParameters["Id"].ParameterName},{baseParameters["Deleted"].ParameterName},{baseParameters["EntityModifiedOn"].ParameterName},{baseParameters["ModifiedBy"].ParameterName}," +
                $"{treatmentId.ParameterName},{recipientTypeId.ParameterName},{recipientType.ParameterName},{recipientId.ParameterName}," +
                $"{notes.ParameterName},{datePerformed.ParameterName},{dosageAmount.ParameterName},{dosageUnitId.ParameterName},{appliedMethod.ParameterName})";
                foreach (var model in returned.Item2)
                
                {
                    if (model is null) continue;
                    PopulateBaseModelParameters(baseParameters, model);
                    treatmentId.Value = model.TreatmentId;
                    recipientTypeId.Value = model.RecipientTypeId;
                    recipientType.Value = model.RecipientType;
                    recipientId.Value = model.RecipientId;
                    notes.Value = model.Notes ?? string.Empty;
                    datePerformed.Value = model.DatePerformed;
                    dosageAmount.Value = model.DosageAmount;
                    dosageUnitId.Value = model.DosageUnitId;
                    appliedMethod.Value = model.AppliedMethod;
                    await command.ExecuteNonQueryAsync();
                }
            }
            
        }

        public async static Task BulkUpdateDutyChore(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(ChoreModel)) && !ShouldEntityBeUpdated(entityModels, nameof(DutyModel))) return;
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

        public async static Task BulkUpdateDutyEvent(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(EventModel)) && !ShouldEntityBeUpdated(entityModels, nameof(DutyModel))) return;
            var dataReceived = new List<DutyEvent>();
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<DutyEvent, GetDutyEventList>("api/GetDutyEventList", new GetDutyEventList { Skip = (int)totalCount });
                expectedCount=returned.Item1;
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
        public async static Task BulkUpdateDutyMilestone(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(MilestoneModel)) && !ShouldEntityBeUpdated(entityModels, nameof(DutyModel))) return;
            var dataReceived=new List<DutyMilestone>();
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<DutyMilestone, GetDutyMilestoneList>("api/GetDutyMilestoneList", new GetDutyMilestoneList { Skip = (int)totalCount });
                expectedCount=returned.Item1;
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
            
            if(typeof(T)==typeof(BreedingRecordModel))
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
