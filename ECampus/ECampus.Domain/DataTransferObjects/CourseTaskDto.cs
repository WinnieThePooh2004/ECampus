using ECampus.Domain.Data;
using ECampus.Domain.Entities;
using ECampus.Domain.Enums;
using ECampus.Domain.Metadata;

namespace ECampus.Domain.DataTransferObjects;

[Dto<CourseTask>]
[Validation]
public class CourseTaskDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime Deadline { get; set; } = DateTime.Now + TimeSpan.FromHours(1);
    public bool ValidAfterDeadline { get; set; }
    public int MaxPoints { get; set; }
    public TaskType Type { get; set; }
    public double Coefficient { get; set; } = 1;
    public int CourseId { get; set; }
}