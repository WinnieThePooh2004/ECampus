using ECampus.Domain.Data;

namespace ECampus.Domain.Models;

public class TaskSubmission : IModel
{
    public int Id { get; set; }
    public int TotalPoints { get; set; }
    public string SubmissionContent { get; set; } = string.Empty;
    public bool IsMarked { get; set; }

    public int CourseTaskId { get; set; }
    public int StudentId { get; set; }
    public CourseTask? CourseTask { get; set; }
    public Student? Student { get; set; }

    public double AbsolutePoints() => CourseTask!.Coefficient * TotalPoints;
}