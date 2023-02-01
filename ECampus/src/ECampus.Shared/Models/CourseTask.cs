using ECampus.Shared.Data;
using ECampus.Shared.Enums;

namespace ECampus.Shared.Models;

public class CourseTask : IModel, IIsDeleted
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool IsDeleted { get; set; }
    public DateTime Deadline { get; set; }
    public bool ValidAfterDeadline { get; set; }
    public int MaxPoints { get; set; }
    public TaskType Type { get; set; }
    
    public int CourseId { get; set; }
    public Course? Course { get; set; }
    public List<TaskSubmission>? Submissions { get; set; }
}