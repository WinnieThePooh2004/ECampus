namespace UniversityTimetable.Shared.Interfaces.Data;

public interface IUpdateValidator<in TDto>
    where TDto : class, IDataTransferObject
{
    Task<Dictionary<string, string>> ValidateAsync(TDto dataTransferObject);
}