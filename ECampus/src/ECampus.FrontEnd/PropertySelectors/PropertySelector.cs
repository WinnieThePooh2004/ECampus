using System.Reflection;
using ECampus.Shared.Extensions;
using ECampus.Shared.Metadata;

namespace ECampus.FrontEnd.PropertySelectors;

public class PropertySelector<T> : IPropertySelector<T>
{
    private readonly List<(PropertyInfo Property, string DisplayName)> _typeProperties;

    public PropertySelector()
    {
        _typeProperties = typeof(T).GetProperties().Where(property =>
                !property.Name.Contains("Id") && (property.PropertyType.IsPrimitive ||
                                                  property.PropertyType.IsEnum ||
                                                  property.PropertyType == typeof(string) ||
                                                  property.GetCustomAttributes(false).OfType<DisplayNameAttribute>()
                                                      .Any()) &&
                !property.GetCustomAttributes().OfType<NotDisplayAttribute>().Any())
            .OrderBy(property => property.DisplayOrder())
            .Select(property => (property, property.DisplayName())).ToList();
    }

    public List<(string DisplayName, string PropertyName)> GetAllPropertiesNames()
    {
        return _typeProperties.Select(property => (property.DisplayName, property.Property.Name)).ToList();
    }

    public List<(string DisplayName, string Value)> GetAllProperties(T item)
    {
        return _typeProperties.Select(property =>
            (displayName: property.DisplayName, GetPropertyValue(property.Property, item))).ToList();
    }

    public List<string> GetAllPropertiesValues(T item)
    {
        return _typeProperties.Select(property =>
            GetPropertyValue(property.Property, item)).ToList();
    }

    private static string GetPropertyValue(PropertyInfo property, T item)
    {
        return property.GetMethod?.Invoke(item, null)?.ToString() ?? "";
    }
}