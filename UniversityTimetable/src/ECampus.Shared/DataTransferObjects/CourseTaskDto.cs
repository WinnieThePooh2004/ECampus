using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;

namespace ECampus.Shared.DataTransferObjects;

[Dto<CourseTask>]
public class CourseTaskDto : IDataTransferObject
{
    public int Id { get; set; }

    [DisplayName("Max points")]
    public int MaxPoints { get; set; }

    public int CourseTaskId { get; set; }
    public int StudentId { get; set; }
}