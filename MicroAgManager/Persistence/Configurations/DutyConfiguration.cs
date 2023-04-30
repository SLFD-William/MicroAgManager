using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations;

namespace BackEnd.Persistence.Configurations
{
    public class DutyConfiguration : BaseEntityConfiguration<Duty>
    {
        public override void Configure(EntityTypeBuilder<Duty> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(40);
            builder.Property(e => e.DaysDue).IsRequired();
            builder.Property(e => e.DutyType).IsRequired().HasMaxLength(20);
            builder.Property(e => e.DutyTypeId).IsRequired();
            builder.Property(e => e.Relationship).HasMaxLength(20);
            builder.Property(e => e.Gender).HasMaxLength(1);
            builder.Property(e => e.SystemRequired).IsRequired();
        }
    }
}
