using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations;

namespace BackEnd.Persistence.Configurations
{
    public class LivestockConfiguration : BaseEntityConfiguration<Livestock>
    {
        public override void Configure(EntityTypeBuilder<Livestock> builder)
        {
            base.Configure(builder);
            builder.HasIndex(e => e.Birthdate, "Index_Birthday");
            builder.HasIndex(e => e.Name, "Index_Name");
            builder.HasIndex(e => e.BeingManaged, "Index_Animal_BeingManaged");

            builder.Property(e => e.BeingManaged).IsRequired();
            builder.Property(e => e.Sterile).IsRequired();
            builder.Property(e => e.InMilk).IsRequired();
            builder.Property(e => e.BottleFed).IsRequired();
            builder.Property(e => e.BornDefective).IsRequired();
            builder.Property(e => e.ForSale).IsRequired();
            builder.Property(e => e.Birthdate).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(255);
            builder.Property(e => e.BirthDefect).HasMaxLength(255);
            builder.Property(e => e.Gender).IsRequired().HasMaxLength(1);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(40);
            builder.Property(e => e.Variety).HasMaxLength(40);
            builder.HasOne(p => p.Father)
           .WithOne()
           .HasForeignKey<Livestock>(a => a.FatherId);
            builder.Property(e => e.MotherId);
            builder.HasOne(p => p.Mother)
            .WithOne()
            .HasForeignKey<Livestock>(a => a.MotherId);
        }
    }
}
