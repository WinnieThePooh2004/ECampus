using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.FrontEnd.Requests.Interfaces.Validation;

public interface IValidationRequests<in T>
{
    Task<ValidationResult> ValidateAsync(T data);
}