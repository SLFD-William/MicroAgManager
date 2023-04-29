using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations;

namespace BackEnd.Persistence.Configurations
{
    public class LivestockFeedAnalysisParameterConfiguration : BaseEntityConfiguration<LivestockFeedAnalysisParameter>
    {
        public override void Configure(EntityTypeBuilder<LivestockFeedAnalysisParameter> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Parameter).IsRequired().HasMaxLength(50);
            builder.Property(e => e.SubParameter).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Unit).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Method).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ReportOrder).IsRequired();
        }
    }
}
