namespace UniversityTimetable.Shared.Interfaces.Data.Validation;

public interface IValidationFacade<in T>
    where T : class
{
    Task<List<KeyValuePair<string, string>>> ValidateCreate(T instance);
    Task<List<KeyValuePair<string, string>>> ValidateUpdate(T instance);
}