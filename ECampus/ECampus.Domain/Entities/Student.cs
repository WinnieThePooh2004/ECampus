﻿using ECampus.Domain.Data;

namespace ECampus.Domain.Entities;

public class Student : IEntity, IIsDeleted
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public int GroupId { get; set; }
    public string? UserEmail { get; set; }
    
    public Group? Group { get; set; }
    public User? User { get; set; }
    
    public List<TaskSubmission>? Submissions { get; set; }
    public List<TeacherRate>? Rates { get; set; }
}