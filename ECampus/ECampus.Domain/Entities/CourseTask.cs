﻿using ECampus.Domain.Data;
using ECampus.Domain.Enums;

namespace ECampus.Domain.Entities;

public class CourseTask : IEntity, IIsDeleted
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool IsDeleted { get; set; }
    public DateTime Deadline { get; set; }
    public bool ValidAfterDeadline { get; set; }
    public int MaxPoints { get; set; }
    public TaskType Type { get; set; }

    public double Coefficient { get; set; } = 1;
    
    public int CourseId { get; set; }
    public Course? Course { get; set; }
    public List<TaskSubmission>? Submissions { get; set; }
}