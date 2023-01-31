using ECampus.Infrastructure.DataUpdateServices;
using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Models;
using ECampus.Shared.Models.RelationModels;
using ECampus.Tests.Unit.InMemoryDb;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.DataUpdate;

public class CourseUpdateServiceTests
{
    private readonly CourseUpdateService _sut;
    private static bool _dataCreated;

    private readonly IDataUpdateService<Course> _baseUpdate =
        Substitute.For<IDataUpdateService<Course>>();

    public CourseUpdateServiceTests()
    {
        _sut = new CourseUpdateService(_baseUpdate);
    }

    [Fact]
    public async Task Update_ShouldInstantlyReturnFromBaseUpdate_WhenGroupsAreNull()
    {
        var context = Substitute.For<DbContext>();
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
        var course = new Course { Id = 11, Groups = new List<Group>() };

        await _sut.UpdateAsync(course, context);
        await context.SaveChangesAsync();

        (await context.TaskSubmissions.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Update_ShouldUpdateRelatedSubmissions()
    {
        await SeedData();
        await using var context = await InMemoryDbFactory.GetContext();
        var course = new Course { Id = 11, Groups = new List<Group> { new() { Id = 10 }, new() { Id = 12 } } };
        var expectedSubmissions = new List<TaskSubmission>
        {
            new() { Id = 1, StudentId = 1, CourseTaskId = 1 }, new() { Id = 2, StudentId = 1, CourseTaskId = 2 },
            new() { Id = 3, StudentId = 2, CourseTaskId = 1 }, new() { Id = 4, StudentId = 2, CourseTaskId = 2 },
            new() { Id = 9, StudentId = 5, CourseTaskId = 1 }, new() { Id = 11, StudentId = 5, CourseTaskId = 2 },
            new() { Id = 10, StudentId = 6, CourseTaskId = 1 }, new() { Id = 12, StudentId = 6, CourseTaskId = 2 }
        };

        await _sut.UpdateAsync(course, context);
        await context.SaveChangesAsync();

        var submissionsAfterUpdate = await context.TaskSubmissions
            .Select(s => new TaskSubmission { StudentId = s.StudentId, Id = s.Id, CourseTaskId = s.CourseTaskId })
            .ToListAsync();
        submissionsAfterUpdate.Should().BeEquivalentTo(expectedSubmissions);
    }

    private static async Task SeedData()
    {
        await using var context = await InMemoryDbFactory.GetContext();
        if (!_dataCreated)
        {
            context.Add(Course);
            context.AddRange(Groups);
            _dataCreated = true;
        }

        context.AddRange(Submissions);
        await context.SaveChangesAsync();
    }

    private static IEnumerable<Group> Groups => new List<Group>
    {
        new()
        {
            Id = 10,
            Students = new List<Student> { new(), new() },
            CourseGroups = new List<CourseGroup> { new() { CourseId = 11 } }
        },
        new()
        {
            Id = 11,
            Students = new List<Student> { new(), new() },
            CourseGroups = new List<CourseGroup> { new() { CourseId = 11 } }
        },
        new()
        {
            Id = 12,
            Students = new List<Student> { new(), new() }
        }
    };

    private static IEnumerable<TaskSubmission> Submissions => Enumerable.Range(0, 8)
        .Select(i => new TaskSubmission { Id = i + 1, StudentId = i / 2 + 1, CourseTaskId = 1 + i % 2 })
        .ToList();

    private static Course Course => new()
    {
        Id = 11,
        Name = "name",
        Tasks = new List<CourseTask>
        {
            new() { Id = 1, Name = "Name11" },
            new() { Id = 2, Name = "Name10" }
        }
    };
}