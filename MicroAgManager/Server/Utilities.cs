using System.Collections;

namespace BackEnd
{
    public static class Utilities
    {
        static public void MapObjectToObject(object to, object from)
        {
            if (to is null || from is null)
                return;
            foreach (var prop in from.GetType().GetProperties().ToList())
            {
                if (!(prop.CanWrite && !(typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))))
                    continue;

                var value = from.GetType().GetProperty(prop.Name).GetValue(from, null);
                var entProp = to.GetType().GetProperty(prop.Name);
                if (entProp is null)
                    continue;
                if (prop.PropertyType.GetNonNullableType() == entProp.PropertyType.GetNonNullableType())
                    entProp.SetValue(to, value);
            }
        }
        public static Type GetNonNullableType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }
    }
}
