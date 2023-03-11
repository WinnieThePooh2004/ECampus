using ECampus.Domain.Data;
using ECampus.Domain.Entities.RelationEntities;
using ECampus.Domain.Metadata.Relationships;

namespace ECampus.Domain.Entities;

[ManyToMany(typeof(Teacher), typeof(CourseTeacher))]
[ManyToMany(typeof(Group), typeof(CourseGroup))]
public class Course : IEntity, IIsDeleted
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }

    public string Name { get; set; } = default!;

    public int SubjectId { get; set; }
    
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now + TimeSpan.FromDays(150);
    
    public List<Teacher>? Teachers { get; set; }
    public Subject? Subject { get; set; }
    public List<Group>? Groups { get; set; }
    
    public List<CourseGroup>? CourseGroups { get; set; }
    public List<CourseTeacher>? CourseTeachers { get; set; }
    public List<CourseTask>? Tasks { get; set; }
    
    public List<TeacherRate>? Rates { get; set; }
}