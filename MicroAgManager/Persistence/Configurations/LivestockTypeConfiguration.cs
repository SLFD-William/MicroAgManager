using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations;

namespace BackEnd.Persistence.Configurations
{
    public class LivestockTypeConfiguration : BaseEntityConfiguration<LivestockType>
    {

        public override void Configure(EntityTypeBuilder<LivestockType> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(40);
            builder.Property(e => e.GroupName).IsRequired().HasMaxLength(40);
            builder.Property(e => e.ParentMaleName).IsRequired().HasMaxLength(40);
            builder.Property(e => e.ParentFemaleName).IsRequired().HasMaxLength(40);
            builder.HasIndex(b => b.Name).IsUnique();
            builder.Property(e => e.Care).IsRequired().HasMaxLength(40);
        }
    }
}
