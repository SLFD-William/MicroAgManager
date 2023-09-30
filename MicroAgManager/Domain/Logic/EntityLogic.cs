using Domain.Abstracts;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Domain.Logic
{
    public static class EntityLogic
    {
        public static Task<List<ModifiedEntity>> GetModifiedEntities(IMicroAgManagementDbContext context)
        {
            var entitiesModified = new List<ModifiedEntity>();
            var changes = ((DbContext)context).ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();
            foreach (var change in changes)
            {
                if (change.Entity as BaseEntity is null) continue;
                var modification = change.State == EntityState.Added ? "Created" : "Modified";
                var entityName = change.Entity.GetType().Name;
                var entityId = change.Entity.GetType().GetProperty("Id").GetValue(change.Entity);
                var modifiedBy = change.Entity.GetType().GetProperty("ModifiedBy").GetValue(change.Entity);
                entitiesModified.Add(new ModifiedEntity(entityId.ToString(), entityName, modification, (Guid)modifiedBy));
            }
            return Task.FromResult(entitiesModified);
        }
    }
}
