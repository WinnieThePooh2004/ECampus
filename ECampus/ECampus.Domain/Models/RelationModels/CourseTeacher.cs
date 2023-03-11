using System.ComponentModel.DataAnnotations;
using ECampus.Domain.Metadata.Relationships;

namespace ECampus.Domain.Models.RelationModels;

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