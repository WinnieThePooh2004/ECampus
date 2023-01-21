﻿using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Models;

public class Student : IModel, IIsDeleted
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public int GroupId { get; set; }
    public int? UserId { get; set; }
    
    public Group? Group { get; set; }
    public User? User { get; set; }
    
    public List<TaskSubmission>? Submissions { get; set; }
}