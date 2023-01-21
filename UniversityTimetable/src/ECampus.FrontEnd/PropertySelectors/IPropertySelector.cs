namespace ECampus.FrontEnd.PropertySelectors;

public interface IPropertySelector<in T>
{
    List<(string displayName, string propertyName)> GetAllPropertiesNames();

    List<(string displayName, string value)> GetAllProperties(T item);

    List<string> GetAllPropertiesValues(T item);
}