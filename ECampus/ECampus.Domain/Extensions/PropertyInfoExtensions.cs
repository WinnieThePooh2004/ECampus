using System.Reflection;
using ECampus.Domain.Metadata;

namespace ECampus.Domain.Extensions;

public static class PropertyInfoExtensions
{
    public static int DisplayOrder(this PropertyInfo property)
    {
        return property.GetCustomAttributes(false).OfType<DisplayNameAttribute>().SingleOrDefault()?.DisplayOrder ?? int.MaxValue;
    }

    public static string DisplayName(this PropertyInfo property)
    {
        var displayName = property.GetCustomAttributes(false).OfType<DisplayNameAttribute>().SingleOrDefault()?.Name;
        return string.IsNullOrWhiteSpace(displayName) ? property.Name : displayName;
    }

    public static TProperty? GetFromProperty<TProperty>(this PropertyInfo property, object item)
    {
        return (TProperty?)property.GetMethod?.Invoke(item, null);
    }

    public static void SetFromProperty<TProperty, TItem>(this PropertyInfo property, TItem item, TProperty? value)
    {
        property.SetMethod!.Invoke(item, new object?[] { value });
    }

    public static void SetPropertyAsNull<TItem>(this PropertyInfo property, TItem item)
    {
        property.SetFromProperty<object, TItem>(item, null);
    }
}