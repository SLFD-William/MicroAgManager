using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations;

namespace BackEnd.Persistence.Configurations
{
    public class MilestoneConfiguration : BaseEntityConfiguration<Milestone>
    {
        public override void Configure(EntityTypeBuilder<Milestone> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Subcategory).IsRequired().HasMaxLength(40);
            builder.Property(e => e.SystemRequired).IsRequired();
        }
    }
}
