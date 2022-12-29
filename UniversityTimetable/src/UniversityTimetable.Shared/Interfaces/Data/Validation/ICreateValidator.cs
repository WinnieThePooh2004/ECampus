using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Interfaces.Data.Validation;

public interface ICreateValidator<in TDto>
    where TDto : class
{
    Task<List<KeyValuePair<string, string>>> ValidateAsync(TDto dataTransferObject);
}