using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace UniversityTimetable.Shared.Comparing
{
    public class PropertyComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T self, T to)
        {
            if (self is null && to is null)
            {
                return true;
            }
            if (self is null || to is null)
            {
                return false;
            }
            Type type = typeof(T);
            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                object toValue = type.GetProperty(pi.Name).GetValue(to, null);

                if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            var idProperty = typeof(T).GetProperty("Id");
            var id = (int)idProperty.GetValue(obj, null);
            return id;
        }
    }
}
