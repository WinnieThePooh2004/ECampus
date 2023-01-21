using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Metadata;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.DataTransferObjects;

[Dto<CourseTask>]
public class CourseTaskDto : IDataTransferObject
{
    public int Id { get; set; }

    [DisplayName("Max points")]
    public int MaxPoints { get; set; }

    public int CourseTaskId { get; set; }
    public int StudentId { get; set; }
}