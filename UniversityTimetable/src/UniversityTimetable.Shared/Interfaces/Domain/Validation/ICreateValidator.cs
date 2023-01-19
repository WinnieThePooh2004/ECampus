using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.Shared.Interfaces.Domain.Validation;

public interface ICreateValidator<in TDto>
{
    Task<ValidationResult> ValidateAsync(TDto dataTransferObject);
}