using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.GuidId).IsRequired();
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.Deleted);
            builder.Property(e => e.DeletedBy);
            builder.Property(e => e.ModifiedOn).IsRequired();
            builder.Property(e => e.ModifiedBy).IsRequired();
            builder.Property(e => e.TenantUserAdminId).IsRequired();
            builder.Property(e => e.Name).IsRequired().HasMaxLength(40);
            builder.Property(e => e.AccessLevel).IsRequired().HasMaxLength(50);
        }
    }
}
