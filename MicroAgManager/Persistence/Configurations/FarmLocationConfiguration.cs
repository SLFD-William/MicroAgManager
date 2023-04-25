using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class FarmLocationConfiguration : BaseEntityConfiguration<FarmLocation>
    {
        public override void Configure(EntityTypeBuilder<FarmLocation> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Longitude);
            builder.Property(e => e.Latitude);
            builder.Property(e => e.StreetAddress);
            builder.Property(e => e.City);
            builder.Property(e => e.State);
            builder.Property(e => e.Zip);
            builder.Property(e => e.Country);
        }
    }
}
