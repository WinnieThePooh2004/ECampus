using System.Text.Json.Serialization;
using AutoMapper.Configuration.Annotations;
using ECampus.Shared.Data;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;

namespace ECampus.Shared.DataTransferObjects;

[Dto(typeof(Course), InjectBaseService = false)]
public class CourseSummary : IDataTransferObject
{
    [NotDisplay] public int CourseId { get; set; }
    [DisplayName("Course name", 0)] public string Name { get; set; } = default!;
    [NotDisplay] 
    public double ScoredPoints { get; set; }
    [NotDisplay] public double MaxPoints { get; set; }
    [DisplayName("Course teachers", 1)] public string TeacherNames { get; set; } = default!;
    [DisplayName("Course started", 2)] public DateTime StartDate { get; set; }
    [DisplayName("Course ends", 3)] public DateTime EndDate { get; set; }

    [Ignore]
    [JsonIgnore]
    [DisplayName("Your points", 4)]
    public string Points => $"{ScoredPoints}/{MaxPoints}";

    int IDataTransferObject.Id
    {
        get => CourseId;
        set => CourseId = value;
    }
}