using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Models;

public class TaskSubmission : IModel, IIsDeleted
{
    public int Id { get; set; }

    public int TotalPoints { get; set; }

    public string SubmissionContent { get; set; } = default!;
    
    public int CourseTaskId { get; set; }
    public int StudentId { get; set; }
    
    public bool IsDeleted { get; set; }
    public CourseTask? CourseTask { get; set; }
    public Student? Student { get; set; }
}