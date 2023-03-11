﻿using ECampus.Domain.Data;
using ECampus.Domain.Metadata;
using ECampus.Domain.Models;

namespace ECampus.Domain.DataTransferObjects;

[Dto<Course>]
[Validation]
public class CourseDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now + TimeSpan.FromDays(150);

    public int SubjectId { get; set; }
    
    public List<GroupDto>? Groups { get; set; }
    public List<TeacherDto>? Teachers { get; set; }
}