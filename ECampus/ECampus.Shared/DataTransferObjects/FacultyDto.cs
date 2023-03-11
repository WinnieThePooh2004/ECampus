using ECampus.Shared.Data;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;

namespace ECampus.Shared.DataTransferObjects;

[Dto<Faculty>]
[Validation]
public class FacultyDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}