using ECampus.Domain.Data;
using ECampus.Domain.Enums;

namespace ECampus.Domain.Entities;

public class TeacherRate : IEntity
{
    public int Id { get; set; }
    public int TeacherId { get; set; }
    public int CourseId { get; set; }
    public string? Feedback { get; set; }
    public int StudentId { get; set; }
    public Dictionary<RateType, int> Rates { get; set; } = new();
    public byte TotalRate { get; set; }
    public bool TeacherIsAcceptable { get; set; }
    public byte KnowledgeEsteem { get; set; }
    
    public Teacher? Teacher { get; set; }
    public Course? Course { get; set; }
    public Student? Student { get; set; }

    public static Dictionary<RateType, int> DefaultRates() =>
        new(Enum.GetValues<RateType>().Select(type => KeyValuePair.Create(type, 0)));
}