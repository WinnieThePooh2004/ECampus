using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Shared.Models;

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
}