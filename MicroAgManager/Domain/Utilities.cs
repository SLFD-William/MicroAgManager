using System.Linq.Expressions;
using System.Reflection;

namespace Domain
{
    public static class Utilities
    {
        public static Type GetNonNullableType(this Type type) =>
            Nullable.GetUnderlyingType(type) ?? type;
        public static MemberInfo GetPropertyMemberInfo<T>(this Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                return null;
            }

            if (!(expression.Body is MemberExpression body))
            {
                UnaryExpression ubody = (UnaryExpression)expression.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body?.Member;
        }
        public static Type GetMemberUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                default:
                    throw new ArgumentException("MemberInfo must be if type FieldInfo, PropertyInfo or EventInfo", nameof(member));
            }
        }
        public static bool IsNumeric(this Type type)
        {
//return true if type is numeric
//also check if type is nullable numeric
            type = type.GetNonNullableType();
            return type == typeof(int) ||
                   type == typeof(double) ||
                   type == typeof(decimal) ||
                   type == typeof(long) ||
                   type == typeof(short) ||
                   type == typeof(sbyte) ||
                   type == typeof(byte) ||
                   type == typeof(ulong) ||
                   type == typeof(ushort) ||
                   type == typeof(uint) ||
                   type == typeof(float);
        }
        public static IOrderedQueryable<T> ApplyOrder<T>(
                IQueryable<T> source,
                string property,
                string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }
}
