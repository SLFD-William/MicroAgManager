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
                var changedEntity = change.Entity as BaseEntity;
                if (changedEntity is null) continue;
                var modification = change.State == EntityState.Added ? "Created" : "Modified";
                entitiesModified.Add(
                    new ModifiedEntity(
                        changedEntity.Id.ToString(), 
                        changedEntity.GetType().Name, 
                        modification,
                        changedEntity.ModifiedBy, 
                        changedEntity.ModifiedOn));
            }
            return Task.FromResult(entitiesModified);
        }
    }
}
