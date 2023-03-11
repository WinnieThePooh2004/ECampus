using ECampus.Infrastructure;
using ECampus.Shared.Models;
using ECampus.Shared.Models.RelationModels;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.Tests.BackEnd.Infrastructure;

// this class was created to increase tests coverage, do not touch it
public class ApplicationDbContextGettersTests
{
    private readonly ApplicationDbContext _sut = Substitute.For<ApplicationDbContext>();

    [Fact]
    public void GetDbSets_ShouldBeInternalDbSets_WhenNotInitialized()
    {
        _sut.Departments.Should().BeAssignableTo<DbSet<Department>>();
        _sut.Faculties.Should().BeAssignableTo<DbSet<Faculty>>();
        _sut.UserAuditories.Should().BeAssignableTo<DbSet<UserAuditory>>();
        _sut.UserGroups.Should().BeAssignableTo<DbSet<UserGroup>>();
        _sut.UserTeachers.Should().BeAssignableTo<DbSet<UserTeacher>>();
        _sut.Students.Should().BeAssignableTo<DbSet<Student>>();
        _sut.Courses.Should().BeAssignableTo<DbSet<Course>>();
        _sut.CourseGroups.Should().BeAssignableTo<DbSet<CourseGroup>>();
        _sut.CourseTeachers.Should().BeAssignableTo<DbSet<CourseTeacher>>();
        _sut.CourseTasks.Should().BeAssignableTo<DbSet<CourseTask>>();
    }
}