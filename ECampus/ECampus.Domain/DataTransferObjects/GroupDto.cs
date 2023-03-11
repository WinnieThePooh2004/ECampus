using ECampus.Domain.Data;
using ECampus.Domain.Entities;
using ECampus.Domain.Metadata;

namespace ECampus.Domain.DataTransferObjects;

[Dto<Group>]
[Validation]
public class GroupDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
}