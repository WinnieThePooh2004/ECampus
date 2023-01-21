using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Metadata.Relationships;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Shared.Models;

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