using System.Reflection;

namespace Domain.Enums
{
    public abstract class BaseEnumeration : IComparable
    {
        public string Name { get; private set; }

        public long Id { get; private set; }

        protected BaseEnumeration(long id, string name) => (Id, Name) = (id, name);
        public override string ToString() => Name;
        public static T FromId<T>(long id) where T : BaseEnumeration
        {
            return typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                     .Select(f => f.GetValue(null))
                     .Cast<T>().FirstOrDefault(x => x.Id == id);
        }
        public static IEnumerable<T> GetAll<T>() where T : BaseEnumeration =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                     .Select(f => f.GetValue(null))
                     .Cast<T>();

        public override bool Equals(object obj)
        {
            if (obj is not BaseEnumeration otherValue)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public int CompareTo(object other) => Id.CompareTo(((BaseEnumeration)other).Id);

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(BaseEnumeration left, BaseEnumeration right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(BaseEnumeration left, BaseEnumeration right)
        {
            return !(left == right);
        }

        public static bool operator <(BaseEnumeration left, BaseEnumeration right)
        {
            return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
        }

        public static bool operator <=(BaseEnumeration left, BaseEnumeration right)
        {
            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
        }

        public static bool operator >(BaseEnumeration left, BaseEnumeration right)
        {
            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
        }

        public static bool operator >=(BaseEnumeration left, BaseEnumeration right)
        {
            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
        }
    }
}
