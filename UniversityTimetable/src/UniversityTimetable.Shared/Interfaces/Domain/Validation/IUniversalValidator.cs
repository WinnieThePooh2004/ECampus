namespace UniversityTimetable.Shared.Interfaces.Domain.Validation;

public interface IUniversalValidator<in TDto> : ICreateValidator<TDto>, IUpdateValidator<TDto>
{
    new Task<List<KeyValuePair<string, string>>> ValidateAsync(TDto dataTransferObject);
}