using ECampus.Domain.Data;
using ECampus.Domain.Metadata;
using ECampus.Domain.Models;

namespace ECampus.Domain.DataTransferObjects;

[Dto<Faculty>]
[Validation]
public class FacultyDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}