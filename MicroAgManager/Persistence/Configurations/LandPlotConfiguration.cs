﻿using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class LandPlotConfiguration : BaseEntityConfiguration<LandPlot>
    {
        public override void Configure(EntityTypeBuilder<LandPlot> builder)
        {
            base.Configure(builder);
            builder.Property(e=>e.Name).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Description).IsRequired().HasMaxLength(255);
            builder.Property(e => e.Area).HasPrecision(20, 3);
            builder.OwnEnumeration(e => e.AreaUnit);
            builder.Property(e => e.AreaUnit);
            builder.OwnEnumeration(e => e.Usage);
            builder.Property(e => e.Usage);
            builder.Property(e => e.FarmLocationId).IsRequired();
        }
    }
}