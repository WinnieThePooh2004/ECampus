using ECampus.Shared.Data;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;

namespace ECampus.Shared.DataTransferObjects;

[Dto(typeof(Department))]
[Validation]
public class DepartmentDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public int FacultyId { get; set; }
}