using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations;

namespace BackEnd.Persistence.Configurations
{
    public class LivestockStatusConfiguration : BaseEntityConfiguration<LivestockStatus>
    {
        public override void Configure(EntityTypeBuilder<LivestockStatus> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Status).IsRequired().HasMaxLength(40);
            builder.Property(e => e.BeingManaged).IsRequired().HasMaxLength(10);
            builder.Property(e => e.Sterile).IsRequired().HasMaxLength(10);
            builder.Property(e => e.DefaultStatus).IsRequired();
            builder.Property(e => e.InMilk).IsRequired().HasMaxLength(10);
            builder.Property(e => e.BottleFed).IsRequired().HasMaxLength(10); 
            builder.Property(e => e.ForSale).IsRequired().HasMaxLength(10); 
        }
    }
}
