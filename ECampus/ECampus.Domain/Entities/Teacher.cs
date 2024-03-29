﻿using ECampus.Domain.Data;
using ECampus.Domain.Entities.RelationEntities;
using ECampus.Domain.Enums;
using ECampus.Domain.Metadata.Relationships;

namespace ECampus.Domain.Entities;

[ManyToMany(typeof(Subject), typeof(SubjectTeacher))]
public class Teacher : IIsDeleted, IEntity
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public ScienceDegree ScienceDegree { get; set; }
    public string? UserEmail { get; set; }

    public bool IsDeleted { get; set; }
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public User? User { get; set; }

    public List<SubjectTeacher>? SubjectIds { get; set; }
    public List<Subject>? Subjects { get; set; }
    public List<Class>? Classes { get; set; }
    public List<User>? Users { get; set; }
    public List<UserTeacher>? UsersIds { get; set; }
    public List<CourseTeacher>? CourseTeachers { get; set; }
    public List<Course>? Courses { get; set; }
    public List<TeacherRate>? Rates { get; set; }
}