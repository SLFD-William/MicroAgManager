using Domain.Enums;
using System.ComponentModel;
using System.Reflection;

namespace Domain.Extensions
{
    public static class BaseEnumerationExtensions
    {
        public static string GetDescription<T>(this T soure) where T : BaseEnumeration
        {
            Type type = soure.GetType();

            if (type.Namespace != "MicroAgManagement.Common.Enums")
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            string name = BaseEnumeration.FromId<T>(soure.Id).Name;// Enum.GetName(type, soure);
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
    }

}
