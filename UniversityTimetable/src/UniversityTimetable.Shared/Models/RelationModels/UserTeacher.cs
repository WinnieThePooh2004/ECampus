﻿using System.ComponentModel.DataAnnotations;
using UniversityTimetable.Shared.Metadata.Relationships;

namespace UniversityTimetable.Shared.Models.RelationModels;

public class UserTeacher
{
    [Key]
    [LeftTableId(typeof(User), typeof(Teacher))]
    public int UserId { get; set; }
    
    [Key]
    [RightTableId(typeof(Teacher))]
    public int TeacherId { get; set; }

    public User? User { get; set; }
    public Teacher? Teacher { get; set; }
}