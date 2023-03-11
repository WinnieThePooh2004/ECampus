using ECampus.Domain.Data;
using ECampus.Domain.Entities.RelationEntities;
using ECampus.Domain.Metadata.Relationships;

namespace ECampus.Domain.Entities;

[ManyToMany(typeof(Teacher), typeof(SubjectTeacher))]
public class Subject : IIsDeleted, IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public List<Class>? Classes { get; set; }
    public List<SubjectTeacher>? TeacherIds { get; set; }
    public List<Teacher>? Teachers { get; set; }
    
    public List<Course>? Courses { get; set; }
}