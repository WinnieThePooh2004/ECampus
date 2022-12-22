using System.Collections.Immutable;
using UniversityTimetable.Infrastructure;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Tests.Shared.TestDatabase
{
    public static class DataSeeder
    {
        public static void SeedData(this ApplicationDbContext context)
        {
            context.Add(Faculty);
            context.AddRange(Subjects);
            context.SaveChanges();
        }

        public static List<Subject> Subjects = new List<Subject>()
        {
            new Subject
            {
                Id = 1,
                Name = "calculus",
                TeacherIds = new List<SubjectTeacher>
                {
                    new SubjectTeacher{ SubjectId = 1, TeacherId = 1 }
                }
            },
            new Subject
            {
                Id = 2,
                Name = "calculus2",
                TeacherIds = new List<SubjectTeacher>
                {
                    new SubjectTeacher{ SubjectId = 2, TeacherId = 2 }
                }
            },
            new Subject
            {
                Id = 3,
                Name = "calculus3",
                TeacherIds = new List<SubjectTeacher>
                {
                    new SubjectTeacher{ SubjectId = 3, TeacherId = 3 }
                }
            }
        };

        public static Faculty Faculty = new Faculty
        {
            Id = 1,
            Name = "Name",
            Departments = new List<Department>
            {
                new Department
                {
                    Id = 1,
                    Name = "Name",
                    FacultyId = 1,
                    Teachers = new List<Teacher>
                    {
                        new Teacher
                        {
                            Id = 1,
                            LastName = "ln1",
                            FirstName = "fn1",
                        },
                        new Teacher
                        {
                            Id = 2,
                            LastName = "ln2",
                            FirstName = "fn2",
                        },
                        new Teacher
                        {
                            Id = 3,
                            LastName = "ln3",
                            FirstName = "fn3",
                        }
                    }
                }
            }
        };
    }
}
