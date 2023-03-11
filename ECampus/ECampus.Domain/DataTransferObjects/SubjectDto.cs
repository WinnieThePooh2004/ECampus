using ECampus.Domain.Data;
using ECampus.Domain.Entities;
using ECampus.Domain.Metadata;

namespace ECampus.Domain.DataTransferObjects;

[Dto<Subject>]
[Validation]
public class SubjectDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<TeacherDto>? Teachers { get; set; }
}