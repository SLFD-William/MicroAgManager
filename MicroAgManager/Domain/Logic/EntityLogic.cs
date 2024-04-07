using Domain.Abstracts;
using Domain.Models;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Domain.Logic
{
    public static class EntityLogic
    {
        public static Task<List<EntityPushNotification>> GetModifiedEntities(DbContext context)
        {
            var entitiesModified = new List<EntityPushNotification>();
            var changes = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();
            foreach (var change in changes)
            {
                var changedEntity = change.Entity as BaseEntity;
                if (changedEntity is null) continue;
                try { 
                var model = CreateModelInstance(changedEntity.GetType().Name + "Model",changedEntity);
                entitiesModified.Add(
                    new EntityPushNotification(
                        changedEntity.TenantId,
                        model.GetJsonString(), 
                        changedEntity.GetType().Name,
                        changedEntity.ModifiedOn
                        ));
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
            return Task.FromResult(entitiesModified);
        }
        public static BaseModel CreateModelInstance(string modelName, BaseEntity entity )
        {
            var type = GetModelType(modelName);
            if (type != null)
            {
                var createMethod = type.GetMethod("ShallowCreate", BindingFlags.Static | BindingFlags.Public);
                if (createMethod == null)
                    createMethod = type.GetMethod("Create", BindingFlags.Static | BindingFlags.Public);

                if (createMethod != null)
                    return createMethod.Invoke(null, [entity]) as BaseModel;

            }

            return null;
        }
        public static Type GetModelType(string entityModelName)
        {
            if (!entityModelName.EndsWith("Model")) entityModelName += "Model";
            switch (entityModelName)
            {
                case "TenantModel":
                    return typeof(TenantModel);
                case "UnitModel":
                    return typeof(UnitModel);
                case "MeasureModel":
                    return typeof(MeasureModel);
                case "TreatmentModel":
                    return typeof(TreatmentModel);
                case "RegistrarModel":
                    return typeof(RegistrarModel);
                case "FarmLocationModel":
                    return typeof(FarmLocationModel);
                case "LandPlotModel":
                    return typeof(LandPlotModel);
                case "LivestockAnimalModel":
                    return typeof(LivestockAnimalModel);
                case "LivestockBreedModel":
                    return typeof(LivestockBreedModel);
                case "LivestockStatusModel":
                    return typeof(LivestockStatusModel);
                case "LivestockModel":
                    return typeof(LivestockModel);
                case "MilestoneModel":
                    return typeof(MilestoneModel);
                case "DutyModel":
                    return typeof(DutyModel);
                case "EventModel":
                    return typeof(EventModel);
                case "ChoreModel":
                    return typeof(ChoreModel);
                case "BreedingRecordModel":
                    return typeof(BreedingRecordModel);
                case "ScheduledDutyModel":
                    return typeof(ScheduledDutyModel);
                case "RegistrationModel":
                    return typeof(RegistrationModel);
                case "MeasurementModel":
                    return typeof(MeasurementModel);
                case "TreatmentRecordModel":
                    return typeof(TreatmentRecordModel);
                default:
                    throw new ArgumentException($"Unknown entity type: {entityModelName}");
            }
        }
    }
}
