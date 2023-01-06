namespace UniversityTimetable.FrontEnd.Requests.Interfaces.Validation;

public interface IValidationRequests<in T>
{
    Task<List<KeyValuePair<string, string>>> ValidateAsync(T data);
}