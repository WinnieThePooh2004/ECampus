using ECampus.Domain.Data;
using ECampus.Domain.Entities;
using ECampus.Domain.Metadata;

namespace ECampus.Domain.DataTransferObjects;

[Dto<Department>]
[Validation]
public class DepartmentDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public int FacultyId { get; set; }
}