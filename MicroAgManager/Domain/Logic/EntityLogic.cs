using Domain.Abstracts;
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
                var modification = change.State == EntityState.Added ? "Created" : "Modified";
                entitiesModified.Add(
                    new EntityPushNotification(
                        changedEntity.TenantId,
                        CreateModelInstance(changedEntity.GetType().Name+"Model").GetJsonString(), 
                        changedEntity.GetType().Name 
                        ));
            }
            return Task.FromResult(entitiesModified);
        }
        public static BaseModel CreateModelInstance(string modelName)
        {
            // Assuming your models are in the "Models" namespace
            var type = Type.GetType($"Models.{modelName}");

            if (type != null)
            {
                var createMethod = type.GetMethod("Create", BindingFlags.Static | BindingFlags.Public);

                if (createMethod != null)
                {
                    return createMethod.Invoke(null, null) as BaseModel;
                }
            }

            return null;
        }

    }
}
