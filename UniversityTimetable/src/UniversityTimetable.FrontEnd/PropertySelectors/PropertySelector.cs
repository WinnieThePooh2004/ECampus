using System.Reflection;
using UniversityTimetable.Shared.Metadata;

namespace UniversityTimetable.FrontEnd.PropertySelectors;

public class PropertySelector<T> : IPropertySelector<T>
{
    private readonly List<(PropertyInfo property, string displayName)> _typeProperties;

    public PropertySelector()
    {
        _typeProperties = typeof(T).GetProperties().Where(property =>
                !property.Name.Contains("Id") && (property.PropertyType.IsPrimitive ||
                                                  property.PropertyType.IsEnum ||
                                                  property.PropertyType == typeof(string)) &&
                !property.GetCustomAttributes().OfType<NotDisplayAttribute>().Any())
            .Select(property => (property,
                property.GetCustomAttributes(false).OfType<DisplayNameAttribute>().SingleOrDefault()?.Name ??
                property.Name)).ToList();
    }

    public List<(string displayName, string propertyName)> GetAllPropertiesNames()
    {
        return _typeProperties.Select(property => (proprtyName: property.displayName, property.property.Name)).ToList();
    }

    public List<string> GetAllPropertiesValues(T item)
    {
        return _typeProperties.Select(property =>
            property.property.GetMethod?.Invoke(item, null)?.ToString() ?? throw new Exception()).ToList();
    }
}