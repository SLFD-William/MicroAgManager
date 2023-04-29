using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations;

namespace BackEnd.Persistence.Configurations
{
    public class LivestockBreedConfiguration : BaseEntityConfiguration<LivestockBreed>
    {
        public override void Configure(EntityTypeBuilder<LivestockBreed> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(40);
            builder.Property(e => e.EmojiChar).HasMaxLength(2);
            builder.Property(e => e.GestationPeriod).IsRequired();
            builder.Property(e => e.HeatPeriod).IsRequired();

        }
    }
}
