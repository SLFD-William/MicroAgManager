using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations;

namespace BackEnd.Persistence.Configurations
{
    public class LivestockFeedDistributionConfiguration : BaseEntityConfiguration<LivestockFeedDistribution>
    {
        public override void Configure(EntityTypeBuilder<LivestockFeedDistribution> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Quantity).HasPrecision(18, 3);
            builder.Property(e => e.Discarded).IsRequired();
            builder.Property(e => e.DatePerformed).IsRequired();
            builder.Property(e => e.Note).HasMaxLength(50);
        }
    }
}
