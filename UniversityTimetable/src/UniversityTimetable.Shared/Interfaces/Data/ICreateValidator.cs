namespace UniversityTimetable.Shared.Interfaces.Data;

public interface ICreateValidator<in TDto>
    where TDto : class, IDataTransferObject
{
    Task<Dictionary<string, string>> ValidateAsync(TDto dataTransferObject);
}