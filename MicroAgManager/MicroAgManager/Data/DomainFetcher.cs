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
        static DbParameter AddNamedParameter(DbCommand command, string name)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            command.Parameters.Add(parameter);
            return parameter;
        }
        private static bool ShouldEntityBeUpdated(List<string>? entityModels, string modelName)=> !(entityModels?.Any() ?? false) || (entityModels?.Contains(modelName) ?? false);
        public async static Task BulkUpdateTenants(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(TenantModel))) return;
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
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.Tenants.FindAsync(model.Id);
                    if (local == null)
                        db.Tenants.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }
            }
        }
        public async static Task BulkUpdateFarmLocations(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(FarmLocationModel))) return;
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
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local=await db.Farms.FindAsync(model.Id);
                    if (local == null)
                        db.Farms.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }
            }
        }
        public async static Task BulkUpdateLivestockAnimals(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(LivestockAnimalModel))) return;
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
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.LivestockAnimals.FindAsync(model.Id);
                    if (local == null)
                        db.LivestockAnimals.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }
            }
        }
        public async static Task BulkUpdateLandPlots(List<string>? entityModels,FrontEndDbContext db, DbConnection connection, IAPIService api)
        {

            if (!ShouldEntityBeUpdated(entityModels, nameof(LandPlotModel))) return;

          
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
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.LandPlots.FindAsync(model.Id);
                    if (local == null)
                        db.LandPlots.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }
            }
        }
        public async static Task BulkUpdateLivestockBreeds(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(LivestockBreedModel))) return;
            
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
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.LivestockBreeds.FindAsync(model.Id);
                    if (local == null)
                        db.LivestockBreeds.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }
            }
        }
        public async static Task BulkUpdateLivestocks(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(LivestockModel)) && !ShouldEntityBeUpdated(entityModels, nameof(BreedingRecordModel))) return;
            
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
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.Livestocks.FindAsync(model.Id);
                    if (local == null)
                        db.Livestocks.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }
                
            }
        }
        public async static Task BulkUpdateLivestockStatuses(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(LivestockStatusModel))) return;
            
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
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.LivestockStatuses.FindAsync(model.Id);
                    if (local == null)
                        db.LivestockStatuses.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();

                }
            }
        }
        public async static Task BulkUpdateMilestones(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(MilestoneModel))) return;
            
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
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.Milestones.FindAsync(model.Id);
                    if (local == null)
                        db.Milestones.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }
            }
        }
        public async static Task BulkUpdateDuties(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(DutyModel))) return;
            
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
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.Duties.FindAsync(model.Id);
                    if (local == null)
                        db.Duties.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async static Task BulkUpdateEvents(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(EventModel))) return;

            var mostRecentUpdate = db.Events.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<EventModel, GetEventList>("api/GetEvents", new GetEventList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                expectedCount = returned.Item1;
                totalCount += returned.Item2.Count;
                expectedCount = returned.Item1;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Events from the API");
                if (expectedCount == 0) break;
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.Events.FindAsync(model.Id);
                    if (local == null)
                        db.Events.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }
            }
        }
        public async static Task BulkUpdateChores(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(ChoreModel))) return;
            
            var mostRecentUpdate = db.Chores.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
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
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.Chores.FindAsync(model.Id);
                    if (local == null)
                        db.Chores.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }
            }
        }
        public async static Task BulkUpdateBreedingRecords(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(BreedingRecordModel))) return;
            
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
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.BreedingRecords.FindAsync(model.Id);
                    if (local == null)
                        db.BreedingRecords.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }
            }
        }
        public async static Task BulkUpdateScheduledDuties(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(ScheduledDutyModel))) return;
            
            var mostRecentUpdate = db.ScheduledDuties.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            if (mostRecentUpdate > DateTime.MinValue.AddDays(1)) mostRecentUpdate = mostRecentUpdate.Value.AddDays(-1);

            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<ScheduledDutyModel, GetScheduledDutyList>("api/GetScheduledDuties", new GetScheduledDutyList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                expectedCount=returned.Item1;
                totalCount += returned.Item2.Count;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Scheduled Duties from the API");
                if (expectedCount == 0) break;
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.ScheduledDuties.FindAsync(model.Id);
                    if (local == null)
                        db.ScheduledDuties.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }

            }
        }
        public async static Task BulkUpdateRegistrars(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(RegistrarModel))) return;
            
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
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.Registrars.FindAsync(model.Id);
                    if (local == null)
                        db.Registrars.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }
            }
        }
        public async static Task BulkUpdateRegistrations(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(RegistrationModel))) return;
            
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
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.Registrations.FindAsync(model.Id);
                    if (local == null)
                        db.Registrations.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
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
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.Units.FindAsync(model.Id);
                    if (local == null)
                        db.Units.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }
            }
        }
        public async static Task BulkUpdateMeasures(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(MeasureModel))) return;
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
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.Measures.FindAsync(model.Id);
                    if (local == null)
                        db.Measures.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }
            }
        }
        public async static Task BulkUpdateMeasurements(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels, nameof(MeasurementModel))) return;
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
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.Measurements.FindAsync(model.Id);
                    if (local == null)
                        db.Measurements.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }
            }
        }
        public async static Task BulkUpdateTreatments(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {

            if (!ShouldEntityBeUpdated(entityModels, nameof(TreatmentModel))) return;
            var mostRecentUpdate = db.Treatments.OrderByDescending(p => p.EntityModifiedOn).FirstOrDefault()?.EntityModifiedOn;
            long totalCount = 0;
            long expectedCount = 1;
            while (totalCount < expectedCount)
            {
                var returned = await api.ProcessQuery<TreatmentModel, GetTreatmentList>("api/GetTreatments", new GetTreatmentList { LastModified = mostRecentUpdate, Skip = (int)totalCount });
                expectedCount=returned.Item1;
                totalCount += returned.Item2.Count;
                Console.WriteLine($"Received {totalCount} of {expectedCount} Treatments from the API");
                foreach (var model in returned.Item2)
                {
                    if (model is null) continue;
                    var local = await db.Treatments.FindAsync(model.Id);
                    if (local == null)
                        db.Treatments.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
                }
            }

        }
        public async static Task BulkUpdateTreatmentRecords(List<string>? entityModels, FrontEndDbContext db, DbConnection connection, IAPIService api)
        {
            if (!ShouldEntityBeUpdated(entityModels,nameof(TreatmentRecordModel))) return;
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
                foreach (var model in returned.Item2)
                
                {
                    if (model is null) continue;
                    var local = await db.TreatmentRecords.FindAsync(model.Id);
                    if (local == null)
                        db.TreatmentRecords.Add(model);
                    else
                        model.Map(local);
                    await db.SaveChangesAsync();
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
