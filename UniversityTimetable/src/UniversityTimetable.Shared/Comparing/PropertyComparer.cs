using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace UniversityTimetable.Shared.Comparing
{
    public class PropertyComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T? self, T? to)
        {
            if (self is null && to is null)
            {
                return true;
            }
            if (self is null || to is null)
            {
                return false;
            }
            var type = typeof(T) ?? throw new UnreachableException("type of T is null");
            for (var index = 0; index < type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Length; index++)
            {
                var pi = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)[index];
                var selfValue = type.GetProperty(pi.Name)?.GetValue(self, null);
                var toValue = type.GetProperty(pi.Name)?.GetValue(to, null);

                if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return obj.GetHashCode();
        }
    }
}
