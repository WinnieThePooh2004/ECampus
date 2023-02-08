using ECampus.Shared.Data;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;

namespace ECampus.Shared.DataTransferObjects;

[Dto(typeof(Log))]
public class LogDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public string MessageTemplate { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Exception { get; set; } = string.Empty;
    public string Properties { get; set; } = string.Empty;
}