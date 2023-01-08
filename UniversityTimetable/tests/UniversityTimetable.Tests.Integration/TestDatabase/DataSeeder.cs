using UniversityTimetable.Infrastructure;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;
using UniversityTimetable.Tests.Integration.AuthHelpers;

namespace UniversityTimetable.Tests.Integration.TestDatabase;

public static class DataSeeder
{
    public static void SeedData(this ApplicationDbContext context)
    {
        context.Add(Faculty);
        context.AddRange(Subjects);
        context.Users.Add(DefaultUsers.Admin);
        context.SaveChanges();
    }

    private static readonly List<Subject> Subjects = new()
    {
        new Subject
        {
            Id = 1,
            Name = "subject1",
            TeacherIds = new List<SubjectTeacher>
            {
                new() { SubjectId = 1, TeacherId = 1 }
            }
        },
        new Subject
        {
            Id = 2,
            Name = "subject2",
            TeacherIds = new List<SubjectTeacher>
            {
                new() { SubjectId = 2, TeacherId = 1 },
                new() { SubjectId = 2, TeacherId = 2 }
            }
        },
        new Subject
        {
            Id = 3,
            Name = "subject3",
            TeacherIds = new List<SubjectTeacher>
            {
                new() { SubjectId = 3, TeacherId = 3 }
            }
        }
    };

    private static readonly Faculty Faculty = new()
    {
        Id = 1,
        Name = "Name",
        Departments = new List<Department>
        {
            new()
            {
                Id = 1,
                Name = "Name",
                FacultyId = 1,
                Teachers = new List<Teacher>
                {
                    new()
                    {
                        Id = 1,
                        LastName = "ln1",
                        FirstName = "fn1"
                    },
                    new()
                    {
                        Id = 2,
                        LastName = "ln2",
                        FirstName = "fn2"
                    },
                    new()
                    {
                        Id = 3,
                        LastName = "ln3",
                        FirstName = "fn3"
                    }
                }
            }
        }
    };
}