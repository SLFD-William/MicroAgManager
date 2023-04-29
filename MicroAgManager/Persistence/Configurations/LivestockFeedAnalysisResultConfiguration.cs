using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations;

namespace BackEnd.Persistence.Configurations
{
    public class LivestockFeedAnalysisResultConfiguration : BaseEntityConfiguration<LivestockFeedAnalysisResult>
    {
        public override void Configure(EntityTypeBuilder<LivestockFeedAnalysisResult> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.AsFed).HasPrecision(18, 2);
            builder.Property(e => e.Dry).HasPrecision(18, 2);
        }
    }
}
