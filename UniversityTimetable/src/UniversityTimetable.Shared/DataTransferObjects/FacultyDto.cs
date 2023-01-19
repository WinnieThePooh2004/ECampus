using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Metadata;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.DataTransferObjects;

[Dto<Faculty>]
public class FacultyDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}