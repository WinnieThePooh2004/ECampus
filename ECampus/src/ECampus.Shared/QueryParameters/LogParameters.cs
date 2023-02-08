using ECampus.Shared.Models;
using Serilog.Events;

namespace ECampus.Shared.QueryParameters;

public class LogParameters : QueryParameters, IDataSelectParameters<Log>
{
    public DateTime From { get; set; } = DateTime.Now - TimeSpan.FromDays(10);
    public DateTime To { get; set; } = DateTime.Now;
    public LogEventLevel MinimalLevel { get; set; } = LogEventLevel.Fatal;
    public LogEventLevel MaxLevel { get; set; } = LogEventLevel.Verbose;
    public bool ExceptionRequired { get; set; }
}