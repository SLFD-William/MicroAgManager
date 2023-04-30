using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations;

namespace BackEnd.Persistence.Configurations
{
    public class EventConfiguration : BaseEntityConfiguration<Event>
    {
        public override void Configure(EntityTypeBuilder<Event> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(40);
            builder.Property(e => e.Color).IsRequired().HasMaxLength(40);
            builder.Property(e => e.StartDate).IsRequired();
            builder.Property(e => e.EndDate);
        }
    }
}
