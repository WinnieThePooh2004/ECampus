using ECampus.Domain.Data;
using ECampus.Domain.Metadata;
using ECampus.Domain.Models;

namespace ECampus.Domain.DataTransferObjects;

[Dto<Department>]
[Validation]
public class DepartmentDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public int FacultyId { get; set; }
}