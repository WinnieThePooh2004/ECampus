using ECampus.Domain.Enums;

namespace ECampus.Domain.Responses.CourseTask;

public class MultipleCourseTaskResponse : IMultipleItemsResponse<Entities.CourseTask>
{
    public int Id { get; set; }
    
    public string Name { get; set; } = default!;
    
    public DateTime Deadline { get; set; } = DateTime.Now + TimeSpan.FromHours(1);
    
    public bool ValidAfterDeadline { get; set; }
    
    public int MaxPoints { get; set; }
    
    public TaskType Type { get; set; }
    
    public double Coefficient { get; set; } = 1;
}