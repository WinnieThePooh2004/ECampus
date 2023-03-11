using ECampus.Domain.Data;
using ECampus.Domain.Metadata.Relationships;
using ECampus.Domain.Models.RelationModels;

namespace ECampus.Domain.Models;

[ManyToMany(typeof(Teacher), typeof(SubjectTeacher))]
public class Subject : IIsDeleted, IModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public List<Class>? Classes { get; set; }
    public List<SubjectTeacher>? TeacherIds { get; set; }
    public List<Teacher>? Teachers { get; set; }
    
    public List<Course>? Courses { get; set; }
}