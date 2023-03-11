using ECampus.Domain.Data;
using ECampus.Domain.Entities;
using ECampus.Domain.Metadata;

namespace ECampus.Domain.DataTransferObjects;

[Dto<TaskSubmission>]
public class TaskSubmissionDto : IDataTransferObject
{
    public int Id { get; set; }
    public int CourseTaskId { get; set; }
    public int StudentId { get; set; }
    public bool IsMarked { get; set; }
    [NotDisplay] public string SubmissionContent { get; set; } = string.Empty;
    
    [DisplayName("Total points", int.MaxValue - 1)] public int TotalPoints { get; set; }
    
    public CourseTaskDto? CourseTask { get; set; }

    public StudentDto? Student { get; set; }
}