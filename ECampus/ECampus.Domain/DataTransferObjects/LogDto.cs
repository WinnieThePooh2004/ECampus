﻿using ECampus.Domain.Data;
using ECampus.Domain.Entities;
using ECampus.Domain.Metadata;
using Serilog.Events;

namespace ECampus.Domain.DataTransferObjects;

[Dto<Log>]
public class LogDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    
    [DisplayName("Time stamp", 0)] public DateTime TimeStamp { get; set; }
    
    [DisplayName("Message template", 1)] public string MessageTemplate { get; set; } = string.Empty;
    
    [DisplayName("Log level", 2)] public LogEventLevel Level { get; set; }
    
    [DisplayName("Exception", 3)] public string Exception { get; set; } = string.Empty;
    public string Properties { get; set; } = string.Empty;
}