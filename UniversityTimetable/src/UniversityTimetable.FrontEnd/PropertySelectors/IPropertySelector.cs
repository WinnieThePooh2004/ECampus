namespace UniversityTimetable.FrontEnd.PropertySelectors;

public interface IPropertySelector<in T>
{
    List<(string displayName, string propertyName)> GetAllPropertiesNames();

    List<string> GetAllPropertiesValues(T item);
}