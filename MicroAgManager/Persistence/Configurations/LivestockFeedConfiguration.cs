using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations;

namespace BackEnd.Persistence.Configurations
{
    public class LivestockFeedConfiguration : BaseEntityConfiguration<LivestockFeed>
    {
        public override void Configure(EntityTypeBuilder<LivestockFeed> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
            builder.Property(e => e.Source).IsRequired().HasMaxLength(255);
            builder.Property(e => e.Cutting);
            builder.Property(e => e.FeedType).IsRequired().HasMaxLength(20);
            builder.Property(e => e.Distribution).IsRequired().HasMaxLength(20);
            builder.Property(e => e.QuantityUnit).IsRequired().HasMaxLength(20);
            builder.Property(e => e.Quantity).HasPrecision(18, 3).IsRequired();
            builder.Property(e => e.QuantityWarning).HasPrecision(18, 3).IsRequired();
            builder.Property(e => e.Active).IsRequired();

        }
    }
}
