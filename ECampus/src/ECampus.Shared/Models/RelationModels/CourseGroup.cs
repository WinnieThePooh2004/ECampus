using System.ComponentModel.DataAnnotations;
using ECampus.Shared.Metadata.Relationships;

namespace ECampus.Shared.Models.RelationModels;

public class CourseGroup
{
    [Key]
    [LeftTableId(typeof(Course))]
    public int CourseId { get; set; }
    
    [Key]
    [RightTableId(typeof(Group))]
    public int GroupId { get; set; }

    public Group? Group { get; set; }
    public Course? Course { get; set; }
}