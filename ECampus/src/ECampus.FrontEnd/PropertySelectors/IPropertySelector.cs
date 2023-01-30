namespace ECampus.FrontEnd.PropertySelectors;

public interface IPropertySelector<in T>
{
    List<(string DisplayName, string PropertyName)> GetAllPropertiesNames();

    List<(string DisplayName, string Value)> GetAllProperties(T item);

    List<string> GetAllPropertiesValues(T item);
}