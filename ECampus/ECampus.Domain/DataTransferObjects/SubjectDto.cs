using ECampus.Domain.Data;
using ECampus.Domain.Entities;
using ECampus.Domain.Metadata;
using ECampus.Domain.Responses.Teacher;

namespace ECampus.Domain.DataTransferObjects;

[Dto<Subject>]
[Validation]
public class SubjectDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<MultipleTeacherResponse>? Teachers { get; set; }
}