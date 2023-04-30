using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations;

namespace BackEnd.Persistence.Configurations
{
    public class ScheduledDutyConfiguration : BaseEntityConfiguration<ScheduledDuty>
    {
        public override void Configure(EntityTypeBuilder<ScheduledDuty> builder)
        {
            base.Configure(builder);
            builder.HasIndex(e => e.CompletedOn, "Index_ScheduledDuty_CompletedOn");
            builder.HasIndex(e => e.DueOn, "Index_ScheduledDuty_DueOn");
            builder.HasIndex(e => e.Dismissed, "Index_ScheduledDuty_Dismissed");

            builder.Property(e => e.ReminderDays).HasPrecision(18, 3); ;
            builder.Property(e => e.CompletedOn);
            builder.Property(e => e.CompletedBy);
            builder.Property(e => e.Dismissed);

        }
    }
}
