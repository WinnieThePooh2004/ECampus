namespace UniversityTimetable.Shared.Interfaces.Domain.Validation;

public interface IUpdateValidator<in TDto>
{
    Task<List<KeyValuePair<string, string>>> ValidateAsync(TDto dataTransferObject);
}