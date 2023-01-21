using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.Models.RelationModels;

namespace ECampus.Shared.Models;

public class Group : IIsDeleted, IModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

    public Department? Department { get; set; }
    public int DepartmentId { get; set; }

    public List<Class>? Classes { get; set; }
    public List<User>? Users { get; set; }
    public List<UserGroup>? UsersIds { get; set; }
    
    public List<Course>? Courses { get; set; }
    public List<CourseGroup>? CourseGroups { get; set; }
    public List<Student>? Students { get; set; }
}