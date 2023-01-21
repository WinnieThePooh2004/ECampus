using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;

namespace ECampus.Shared.DataTransferObjects;

[Dto<Department>]
public class DepartmentDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public int FacultyId { get; set; }
}