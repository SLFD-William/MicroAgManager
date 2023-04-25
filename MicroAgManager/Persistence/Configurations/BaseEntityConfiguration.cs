using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.Deleted);
            builder.Property(e => e.DeletedBy);
            builder.Property(e => e.ModifiedOn).IsRequired();
            builder.Property(e => e.ModifiedBy).IsRequired();
            builder.Property(e => e.TenantId).IsRequired();
            builder.HasIndex(e => new { e.Id, e.TenantId }, $"Index_{typeof(T).Name}_TenantIdAndPrimaryKey");
            builder.HasIndex(e => e.ModifiedOn, $"Index_{typeof(T).Name}_ModifiedOn");
            builder.HasIndex(e => e.Deleted, $"Index_{typeof(T).Name}_Deleted");
        }
    }
}
