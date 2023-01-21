using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;

namespace ECampus.Shared.DataTransferObjects;

[Dto<Subject>]
public class SubjectDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<TeacherDto>? Teachers { get; set; }
}