namespace UniversityTimetable.Shared.Interfaces.Domain.Validation;

public interface ICreateValidator<in TDto>
{
    Task<List<KeyValuePair<string, string>>> ValidateAsync(TDto dataTransferObject);
}