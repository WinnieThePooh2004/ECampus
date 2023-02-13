﻿using ECampus.Shared.Data;
using ECampus.Shared.Enums;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;

namespace ECampus.Shared.DataTransferObjects;

[Dto<TeacherRate>]
[Validation]
public class TeacherRateDto : IDataTransferObject
{
    public int Id { get; set; }
    public int TeacherId { get; set; }
    public int CourseId { get; set; }
    public string? Feedback { get; set; }
    public int StudentId { get; set; }
    public Dictionary<RateType, int> Rates { get; set; } = TeacherRate.DefaultRates();
    public byte TotalRate { get; set; }
    public bool TeacherIsAcceptable { get; set; }
    public byte KnowledgeEsteem { get; set; }
    public string TeachersName { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
}