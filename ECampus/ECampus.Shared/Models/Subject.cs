﻿using ECampus.Shared.Data;
using ECampus.Shared.Metadata.Relationships;
using ECampus.Shared.Models.RelationModels;

namespace ECampus.Shared.Models;

[ManyToMany(typeof(Teacher), typeof(SubjectTeacher))]
public class Subject : IIsDeleted, IModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public List<Class>? Classes { get; set; }
    public List<SubjectTeacher>? TeacherIds { get; set; }
    public List<Teacher>? Teachers { get; set; }
    
    public List<Course>? Courses { get; set; }
}