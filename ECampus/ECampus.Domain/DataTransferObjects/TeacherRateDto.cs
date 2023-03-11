using ECampus.Domain.Data;
using ECampus.Domain.Entities;
using ECampus.Domain.Enums;
using ECampus.Domain.Metadata;

namespace ECampus.Domain.DataTransferObjects;

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