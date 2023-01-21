using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Metadata;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.DataTransferObjects;

[Dto<TaskSubmission>]
public class TaskSubmissionDto : IDataTransferObject
{
    public int Id { get; set; }
    public int CourseTaskId { get; set; }
    public int StudentId { get; set; }
    public string SubmissionContent { get; set; } = string.Empty;
    
    [DisplayName("Total points")]
    public int TotalPoints { get; set; }
    
    public CourseTaskDto? CourseTask { get; set; }
}