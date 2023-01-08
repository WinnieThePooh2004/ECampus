using System.Text.Json.Serialization;
using UniversityTimetable.Shared.Attributes;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.DataTransferObjects;

[Dto<Teacher>]
[Validation]
public class TeacherDto : IDataTransferObject
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public ScienceDegree ScienceDegree { get; set; }

    public int DepartmentId { get; set; }
    public List<SubjectDto>? Subjects { get; set; }
    [JsonIgnore] public string FullName => $"{FirstName[0]}. {LastName}";
}