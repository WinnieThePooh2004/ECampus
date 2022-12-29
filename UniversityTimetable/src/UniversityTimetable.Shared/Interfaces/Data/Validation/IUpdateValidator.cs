namespace UniversityTimetable.Shared.Interfaces.Data.Validation;

public interface IUpdateValidator<in TDto>
{
    Task<List<KeyValuePair<string, string>>> ValidateAsync(TDto dataTransferObject);
}