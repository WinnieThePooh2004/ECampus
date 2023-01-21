using System.ComponentModel.DataAnnotations;
using UniversityTimetable.Shared.Metadata.Relationships;

namespace UniversityTimetable.Shared.Models.RelationModels;

public class CourseTeacher
{
    [Key]
    [RightTableId(typeof(Teacher))]
    public int TeacherId { get; set; }
    
    [Key]
    [LeftTableId(typeof(Course))]
    public int CourseId { get; set; }
    
    public Teacher? Teacher { get; set; }
    public Course? Course { get; set; }
}