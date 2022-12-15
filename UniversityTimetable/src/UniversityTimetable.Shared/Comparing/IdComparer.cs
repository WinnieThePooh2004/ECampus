using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace UniversityTimetable.Shared.Comparing
{
    public class IdComparer<T> : IEqualityComparer<T>
        where T : class
    {
        public bool Equals(T x, T y)
        {
            if(x is null || y is null)
            {
                return x == y;
            }
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            var idProperty = typeof(T).GetProperty("Id");
            var id = (int)idProperty.GetValue(obj, null);
            return id;
        }
    }
}
