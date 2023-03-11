using System.ComponentModel.DataAnnotations;
using ECampus.Shared.Metadata.Relationships;

namespace ECampus.Shared.Models.RelationModels;

public class SubjectTeacher
{
    [Key]
    [LeftTableId(typeof(Teacher))]
    [RightTableId(typeof(Teacher))]
    public int TeacherId { get; set; }
    
    [Key]
    [LeftTableId(typeof(Subject))]
    [RightTableId(typeof(Subject))]
    public int SubjectId { get; set; }

    public Teacher? Teacher { get; set; }
    public Subject? Subject { get; set; }
}