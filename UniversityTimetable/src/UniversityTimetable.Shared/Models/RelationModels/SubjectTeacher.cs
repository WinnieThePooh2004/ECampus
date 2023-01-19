﻿using System.ComponentModel.DataAnnotations;
using UniversityTimetable.Shared.Metadata.Relationships;

namespace UniversityTimetable.Shared.Models.RelationModels;

public class SubjectTeacher
{
    [Key]
    [LeftTableId(typeof(Teacher), typeof(Subject))]
    [RightTableId(typeof(Teacher))]
    public int TeacherId { get; set; }
    
    [Key]
    [LeftTableId(typeof(Subject), typeof(Teacher))]
    [RightTableId(typeof(Subject))]
    public int SubjectId { get; set; }

    public Teacher? Teacher { get; set; }
    public Subject? Subject { get; set; }
}