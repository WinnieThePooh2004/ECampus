using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.DataTransferObjects;

public class AuditoryDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Building { get; set; } = string.Empty;
}