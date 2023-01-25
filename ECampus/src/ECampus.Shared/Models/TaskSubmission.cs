using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.Metadata;

namespace ECampus.Shared.Models;

public class TaskSubmission : IModel, IIsDeleted
{
    public int Id { get; set; }
    
    [DisplayName("Points", int.MaxValue - 1)]
    public int TotalPoints { get; set; }

    [DisplayName("Content")]
    public string SubmissionContent { get; set; } = string.Empty;
    
    public int CourseTaskId { get; set; }
    public int StudentId { get; set; }
    
    public bool IsDeleted { get; set; }
    public CourseTask? CourseTask { get; set; }
    public Student? Student { get; set; }
}