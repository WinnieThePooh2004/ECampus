using UniversityTimetable.Shared.Attributes;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.DataTransferObjects;

[Dto<Faculty>]
[Validation]
public class FacultyDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}