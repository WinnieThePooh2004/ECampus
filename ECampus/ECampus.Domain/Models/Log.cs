using ECampus.Domain.Data;
using Serilog.Events;

namespace ECampus.Domain.Models;

public class Log : IModel
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public string MessageTemplate { get; set; } = string.Empty;
    public LogEventLevel Level { get; set; }
    public string? Exception { get; set; }
    public DateTime TimeStamp { get; set; }
    public string Properties { get; set; } = string.Empty;
}