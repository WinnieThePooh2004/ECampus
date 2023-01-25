using ECampus.Shared.Enums;
using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.Metadata.Relationships;
using ECampus.Shared.Models.RelationModels;

namespace ECampus.Shared.Models;

[ManyToMany(typeof(Subject), typeof(SubjectTeacher))]
public class Teacher : IIsDeleted, IModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public ScienceDegree ScienceDegree { get; set; }
    public string? UserEmail { get; set; }

    public bool IsDeleted { get; set; }
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public User? User { get; set; }

    public List<SubjectTeacher>? SubjectIds { get; set; }
    public List<Subject>? Subjects { get; set; }
    public List<Class>? Classes { get; set; }
    public List<User>? Users { get; set; }
    public List<UserTeacher>? UsersIds { get; set; }
    public List<CourseTeacher>? CourseTeachers { get; set; }
    public List<Course>? Courses { get; set; }
}