using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.Shared.Interfaces.Domain.Validation;

public interface IUpdateValidator<in TDto>
{
    Task<ValidationResult> ValidateAsync(TDto dataTransferObject);
}