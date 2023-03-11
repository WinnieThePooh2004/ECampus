using ECampus.Shared.Data;
using ECampus.Shared.Enums;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;

namespace ECampus.Shared.DataTransferObjects;

[Dto<Class>]
[Validation]
public class ClassDto : IDataTransferObject
{
    public int Id { get; set; }
    public ClassType ClassType { get; set; } = ClassType.Lecture;
    public int Number { get; set; }
    public int DayOfWeek { get; set; }
    public WeekDependency WeekDependency { get; set; } = WeekDependency.None;

    public int TeacherId { get; set; }
    public int GroupId { get; set; }
    public int AuditoryId { get; set; }
    public int SubjectId { get; set; }
    public AuditoryDto? Auditory { get; set; }
    public TeacherDto? Teacher { get; set; }
    public GroupDto? Group { get; set; }
    public SubjectDto? Subject { get; set; }
}