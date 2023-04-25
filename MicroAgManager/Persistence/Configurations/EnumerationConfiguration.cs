using Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace Persistence.Configurations
{
    internal static class EnumerationConfiguration
    {
        public static void OwnEnumeration<TEntity, TEnum>(this EntityTypeBuilder<TEntity> builder,
            Expression<Func<TEntity, TEnum>> property)
            where TEntity : class
            where TEnum : BaseEnumeration
        {
            builder
                .Property(property)
                .HasConversion(x => x.Id, x => BaseEnumeration.FromId<TEnum>(x));
        }
    }
}
