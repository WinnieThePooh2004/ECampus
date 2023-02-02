using ECampus.Infrastructure;
using ECampus.Infrastructure.DataUpdateServices;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;
using ECampus.Shared.Models.RelationModels;
using ECampus.Tests.Unit.InMemoryDb;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.DataUpdate;

public class CourseUpdateServiceTests
{
    private static bool _relatedDataCreated;

    private readonly CourseUpdateService _sut;

    private readonly IDataUpdateService<Course> _baseUpdate =
        Substitute.For<IDataUpdateService<Course>>();

    public CourseUpdateServiceTests()
    {
        _sut = new CourseUpdateService(_baseUpdate);
    }

    [Fact]
    public async Task Update_ShouldInstantlyReturnFromBaseUpdate_WhenGroupsAreNull()
    {
        var context = Substitute.For<ApplicationDbContext>();
        var course = new Course();

        await _sut.UpdateAsync(course, context);

        await _baseUpdate.Received(1).UpdateAsync(course, context);
        context.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public async Task Update_ShouldClearAllRelatedSubmissions_WhenNoGroupsPassed()
    {
        await SeedData();
        await using var context = await InMemoryDbFactory.GetContext();
        var course = new Course { Id = 1, Groups = new List<Group>() };

        await _sut.UpdateAsync(course, context);
        await context.SaveChangesAsync();

        (await context.TaskSubmissions
                .Include(t => t.CourseTask)
                .Where(c => c.CourseTask!.CourseId == 1).CountAsync())
            .Should().Be(0);
    }

    [Fact]
    public async Task Update_ShouldUpdateRelatedSubmissions()
    {
        await SeedData(100);
        await using var context = await InMemoryDbFactory.GetContext();
        var course = new Course { Id = 100, Groups = new List<Group> { new() { Id = 100 }, new() { Id = 102 } } };
        var expectedSubmissions = Enumerable.Range(0, 8).Select(i => new TaskSubmission
        {
            StudentId = i < 4 ? i / 2 + 100 : i / 2 + 102,
            CourseTaskId = 100 + i % 2
        });

        await _sut.UpdateAsync(course, context);
        await context.SaveChangesAsync();

        var submissionsAfterUpdate = await context.TaskSubmissions
            .Include(t => t.CourseTask)
            .Where(c => c.CourseTask!.CourseId == 100)
            .Select(s => new TaskSubmission { StudentId = s.StudentId, CourseTaskId = s.CourseTaskId })
            .ToListAsync();
        submissionsAfterUpdate.Should().BeEquivalentTo(expectedSubmissions);
    }

    private static async Task SeedData(int firstItemId = 1)
    {
        await using var context = await InMemoryDbFactory.GetContext();
        CreateRelatedData(context);
        context.Add(Course(firstItemId));
        context.AddRange(Groups(firstItemId));
        context.AddRange(Submissions(firstItemId));
        await context.SaveChangesAsync();
    }

    private static void CreateRelatedData(DbContext context)
    {
        if (_relatedDataCreated)
        {
            return;
        }

        var faculty = new Faculty
            { Id = 1, Name = "f1", Departments = new List<Department> { new() { Name = "name1", Id = 1 } } };
        context.Add(faculty);
        context.Add(new Subject { Id = 1, Name = "s1" });
        _relatedDataCreated = true;
    }

    private static IEnumerable<Group> Groups(int firstItemId = 1) => new List<Group>
    {
        new()
        {
            Id = firstItemId, DepartmentId = 1,
            Students = new List<Student> { new() { Id = firstItemId }, new() { Id = firstItemId + 1 } },
            CourseGroups = new List<CourseGroup> { new() { CourseId = firstItemId } }
        },
        new()
        {
            Id = 1 + firstItemId, DepartmentId = 1,
            Students = new List<Student> { new() { Id = firstItemId + 2 }, new() { Id = firstItemId + 3 } },
            CourseGroups = new List<CourseGroup> { new() { CourseId = firstItemId } }
        },
        new()
        {
            Id = 2 + firstItemId, DepartmentId = 1,
            Students = new List<Student> { new() { Id = firstItemId + 4 }, new() { Id = firstItemId + 5 } }
        }
    };

    private static IEnumerable<TaskSubmission> Submissions(int firstItemId = 1) => Enumerable.Range(0, 8)
        .Select(i => new TaskSubmission
            { Id = i + firstItemId, StudentId = i / 2 + firstItemId, CourseTaskId = firstItemId + i % 2 })
        .ToList();

    private static Course Course(int firstItemId = 1) => new()
    {
        Id = firstItemId,
        Name = "name",
        SubjectId = 1,
        Tasks = new List<CourseTask>
        {
            new() { Id = firstItemId, Name = "Name11" },
            new() { Id = firstItemId + 1, Name = "Name10" }
        }
    };
}