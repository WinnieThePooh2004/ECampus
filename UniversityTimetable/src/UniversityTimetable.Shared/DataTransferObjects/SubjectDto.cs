using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Metadata;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.DataTransferObjects;

[Dto<Subject>]
public class SubjectDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<TeacherDto>? Teachers { get; set; }
}