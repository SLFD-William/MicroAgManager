using System.ComponentModel;
using System.Reflection;

namespace Domain.Extensions
{
    public static class EnumExtensions
    {
        public static int ToInt<T>(this T soure) where T : IConvertible//enum
        {
            Type type = soure.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return (int)(IConvertible)soure;
        }

        public static int Count<T>(this T soure) where T : IConvertible//enum
        {
            Type type = soure.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return Enum.GetNames(type).Length;
        }
        public static string GetEnumDescription<T>(this T soure)
        {
            Type type = soure.GetType();

            if (!type.IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            string name = Enum.GetName(type, soure);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                    return field.Name;
                }
            }
            return null;
        }

        public static string GetEnumDescriptionFromName<T>(this T soure, string namePassed)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return ((T)Enum.Parse(typeof(T), namePassed)).GetEnumDescription();
        }

        public static IEnumerable<T> GetFilteredBy<T>(this T soure, Func<FieldInfo, bool> filter) where T : IConvertible//enum
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var myreturn = new List<T>();
            Array values = Enum.GetValues(typeof(T));

            if (filter == null)
            {
                foreach (T val in values)
                {
                    myreturn.Add(val);
                }
                return myreturn;
            }
            var selectedFields = typeof(T).GetFields().Where(filter);
            foreach (T val in values)
            {
                if (selectedFields.Contains(typeof(T).GetField(Enum.GetName(typeof(T), val))))
                {
                    myreturn.Add(val);
                }
            }

            return myreturn;
        }
    }
}
