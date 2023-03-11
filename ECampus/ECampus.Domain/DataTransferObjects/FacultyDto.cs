using ECampus.Domain.Data;
using ECampus.Domain.Entities;
using ECampus.Domain.Metadata;

namespace ECampus.Domain.DataTransferObjects;

[Dto<Faculty>]
[Validation]
public class FacultyDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}