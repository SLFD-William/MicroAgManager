using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations;

namespace BackEnd.Persistence.Configurations
{
    public class LivestockFeedServingConfiguration : BaseEntityConfiguration<LivestockFeedServing>
    {
        public override void Configure(EntityTypeBuilder<LivestockFeedServing> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.ServingFrequency).IsRequired().HasMaxLength(50);
            
            builder.Property(e => e.Serving).HasPrecision(18, 3);
        }
    }
}
