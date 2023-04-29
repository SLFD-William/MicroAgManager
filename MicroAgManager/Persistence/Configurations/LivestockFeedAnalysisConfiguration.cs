using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations;

namespace BackEnd.Persistence.Configurations
{
    public class LivestockFeedAnalysisConfiguration : BaseEntityConfiguration<LivestockFeedAnalysis>
    {
        public override void Configure(EntityTypeBuilder<LivestockFeedAnalysis> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.LabNumber).HasMaxLength(40);
            builder.Property(e => e.TestCode).HasMaxLength(40);
            builder.Property(e => e.DateSampled);
            builder.Property(e => e.DateReceived);
            builder.Property(e => e.DateReported);
            builder.Property(e => e.DatePrinted);
        }
    }
}
