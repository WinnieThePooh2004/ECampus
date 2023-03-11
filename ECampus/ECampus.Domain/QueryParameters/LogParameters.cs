using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;
using Serilog.Events;

namespace ECampus.Domain.QueryParameters;

public class LogParameters : QueryParameters<LogDto>, IDataSelectParameters<Log>
{
    public DateTime From { get; set; } = DateTime.Now - TimeSpan.FromDays(10);
    // default value it now + 1 day because of time laps
    public DateTime To { get; set; } = DateTime.Now + TimeSpan.FromDays(1);
    public LogEventLevel MinimalLevel { get; set; } = LogEventLevel.Information;
    public LogEventLevel MaxLevel { get; set; } = LogEventLevel.Error;
}