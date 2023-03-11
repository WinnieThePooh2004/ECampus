using ECampus.Domain.Data;
using ECampus.Domain.Enums;

namespace ECampus.Domain.Entities;

public class Class : IIsDeleted, IEntity
{
    public int Id { get; set; }
    public ClassType ClassType { get; set; }
    public int Number { get; set; }
    public int DayOfWeek { get; set; }
    public bool IsDeleted { get; set; }
    public int SubjectId { get; set; }
    public WeekDependency WeekDependency { get; set; } = WeekDependency.None;
        
    public int TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
    public int GroupId { get; set; }
    public Group? Group { get; set; }
    public int AuditoryId { get; set; }
    public Auditory? Auditory { get; set; }
    public Subject? Subject { get; set; }
}