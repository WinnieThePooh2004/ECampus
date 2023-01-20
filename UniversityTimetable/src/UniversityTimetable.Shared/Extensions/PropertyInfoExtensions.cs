using System.Reflection;
using UniversityTimetable.Shared.Metadata;

namespace UniversityTimetable.Shared.Extensions;

public static class PropertyInfoExtensions
{
    public static int DisplayOrder(this PropertyInfo property)
    {
        return property.GetCustomAttributes(false).OfType<DisplayNameAttribute>().SingleOrDefault()?.DisplayOrder ??
               int.MaxValue;
    }

    public static string DisplayName(this PropertyInfo property)
    {
        var displayName = property.GetCustomAttributes(false).OfType<DisplayNameAttribute>().SingleOrDefault()?.Name;
        return string.IsNullOrWhiteSpace(displayName) ? property.Name : displayName;
    }
}