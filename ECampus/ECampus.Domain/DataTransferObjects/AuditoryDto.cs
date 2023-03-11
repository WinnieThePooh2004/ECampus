using ECampus.Domain.Data;
using ECampus.Domain.Metadata;
using ECampus.Domain.Models;

namespace ECampus.Domain.DataTransferObjects;

[Dto<Auditory>]
[Validation]
public class AuditoryDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Building { get; set; } = string.Empty;
}