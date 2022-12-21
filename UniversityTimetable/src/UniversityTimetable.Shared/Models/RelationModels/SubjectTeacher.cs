using System.ComponentModel.DataAnnotations;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;

namespace UniversityTimetable.Shared.Models.RelationModels;

public class SubjectTeacher : IRelationModel<Teacher, Subject>, IRelationModel<Subject, Teacher>
{
    [Key] public int TeacherId { get; set; }
    [Key] public int SubjectId { get; set; }

    public Teacher Teacher { get; set; }
    public Subject Subject { get; set; }

    int IRelationModel<Subject, Teacher>.RightTableId { get => TeacherId; init => TeacherId = value; }
    int IRelationModel<Subject, Teacher>.LeftTableId { init => SubjectId = value; }
    int IRelationModel<Teacher, Subject>.RightTableId { get => SubjectId; init => SubjectId = value; }
    int IRelationModel<Teacher, Subject>.LeftTableId { init => TeacherId = value; }
}