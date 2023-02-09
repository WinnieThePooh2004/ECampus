﻿using ECampus.Shared.Models;
using Serilog.Events;

namespace ECampus.Shared.QueryParameters;

public class LogParameters : QueryParameters, IDataSelectParameters<Log>
{
    public DateTime From { get; set; } = DateTime.Now - TimeSpan.FromDays(10);
    // default value it now + 1 day because of time laps
    public DateTime To { get; set; } = DateTime.Now + TimeSpan.FromDays(1);
    public LogEventLevel MinimalLevel { get; set; } = LogEventLevel.Verbose;
    public LogEventLevel MaxLevel { get; set; } = LogEventLevel.Fatal;
    // public string Exception { get; set; } = string.Empty;
    // public string Message { get; set; } = string.Empty;
}