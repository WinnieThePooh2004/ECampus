using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;

namespace ECampus.Shared.DataTransferObjects;

[Dto<Course>]
public class CourseDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public List<GroupDto>? Groups { get; set; }
    public List<TeacherDto>? Teachers { get; set; }
}