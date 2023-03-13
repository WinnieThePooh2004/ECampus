using ECampus.Domain.Data;
using ECampus.Domain.Entities;
using ECampus.Domain.Metadata;
using ECampus.Domain.Responses.Group;
using ECampus.Domain.Responses.Teacher;

namespace ECampus.Domain.DataTransferObjects;

[Dto<Course>]
[Validation]
public class CourseDto : IDataTransferObject
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; } = DateTime.Now;
    
    public DateTime EndDate { get; set; } = DateTime.Now + TimeSpan.FromDays(150);

    public int SubjectId { get; set; }
    
    public List<MultipleGroupResponse>? Groups { get; set; }
    public List<MultipleTeacherResponse>? Teachers { get; set; }
}