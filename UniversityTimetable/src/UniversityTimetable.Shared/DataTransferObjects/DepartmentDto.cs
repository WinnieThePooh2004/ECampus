using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Metadata;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.DataTransferObjects;

[Dto<Department>]
public class DepartmentDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public int FacultyId { get; set; }
}