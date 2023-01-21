using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Metadata.Relationships;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Shared.Models;

[ManyToMany(typeof(TeacherDto), typeof(CourseTeacher))]
[ManyToMany(typeof(Group), typeof(CourseGroup))]
public class Course : IModel, IIsDeleted
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }

    public string Name { get; set; } = default!;
    
    public List<Teacher>? Teachers { get; set; }
    public Subject? Subject { get; set; }
    public List<Group>? Groups { get; set; }
    
    public List<CourseGroup>? CourseGroups { get; set; }
    public List<CourseTeacher>? CourseTeachers { get; set; }
    public List<CourseTask>? Tasks { get; set; }
}