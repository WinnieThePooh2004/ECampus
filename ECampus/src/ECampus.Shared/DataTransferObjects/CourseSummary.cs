using ECampus.Shared.Data;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;

namespace ECampus.Shared.DataTransferObjects;

[Dto(typeof(Course), InjectBaseService = false)]
public class CourseSummary : IDataTransferObject
{
    public int CourseId { get; set; }
    public string Name { get; set; } = default!;
    public double TotalPoints { get; set; }
    public string TeacherNames { get; set; } = default!;
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    int IDataTransferObject.Id
    {
        get => CourseId;
        set => CourseId = value;
    }
}