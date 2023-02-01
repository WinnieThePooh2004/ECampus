using ECampus.Shared.Models;
namespace ECampus.Shared.DataTransferObjects;

public class CourseSummary
{
    public int CourseId { get; set; }
    public string Name { get; set; } = default!;
    public double TotalPoints { get; set; }
    public string TeacherNames { get; set; } = default!;

    public CourseSummary()
    {
    }

    public static CourseSummary FromCourse(Course course)
        => new()
        {
            Name = course.Name,
            CourseId = course.Id,
            TotalPoints = course.Tasks!.Select(task => task.Submissions!.Single().Id).Sum(),
            TeacherNames = string.Join(", ",
                course.Teachers!.Select(teacher => $"{teacher.FirstName} {teacher.LastName}"))
        };
}