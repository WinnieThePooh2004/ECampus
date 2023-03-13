using System.Text.Json.Serialization;
using AutoMapper.Configuration.Annotations;
using ECampus.Domain.Metadata;

namespace ECampus.Domain.Responses.Course;

public class CourseSummaryResponse : IMultipleItemsResponse<Entities.Course>
{
    [NotDisplay] public int CourseId { get; set; }
    
    [DisplayName("Course name", 0)] public string Name { get; set; } = default!;
    
    [NotDisplay] public double ScoredPoints { get; set; }
    
    [NotDisplay] public double MaxPoints { get; set; }
    
    [DisplayName("Course teachers", 1)] public string TeacherNames { get; set; } = default!;
    
    [DisplayName("Course started", 2)] public DateTime StartDate { get; set; }
    
    [DisplayName("Course ends", 3)] public DateTime EndDate { get; set; }

    [Ignore]
    [JsonIgnore]
    [DisplayName("Your points", 4)]
    public string Points => $"{ScoredPoints}/{MaxPoints}";

    int IMultipleItemsResponse.Id
    {
        get => CourseId;
        set => CourseId = value;
    }
}