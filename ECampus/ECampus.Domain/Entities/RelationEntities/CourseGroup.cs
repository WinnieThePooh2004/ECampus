using System.ComponentModel.DataAnnotations;
using ECampus.Domain.Metadata.Relationships;

namespace ECampus.Domain.Entities.RelationEntities;

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