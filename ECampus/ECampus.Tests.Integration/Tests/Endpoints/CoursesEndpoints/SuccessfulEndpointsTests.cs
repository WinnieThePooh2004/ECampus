using ECampus.Shared.DataContainers;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using ECampus.Shared.Models.RelationModels;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ECampus.Tests.Integration.Tests.Endpoints.CoursesEndpoints;

public class SuccessfulEndpointsTests : IClassFixture<ApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private static bool _dataCreated;

    public SuccessfulEndpointsTests(ApplicationFactory app)
    {
        _client = app.CreateClient();
    }

    public async Task InitializeAsync()
    {
        if (_dataCreated)
        {
            return;
        }

        _dataCreated = true;
        await SeedData();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetSummary_ShouldReturnFromDb_WhenParametersAreValid()
    {
        var student = DefaultUsers.GetUserByRole(UserRole.Student);
        student.StudentId = 200;
        _client.Login(student);

        var response = await _client.GetAsync(
            $"/api/Courses/summary?{new CourseSummaryParameters
            {
                StudentId = 200, OrderBy = nameof(CourseSummary.StartDate)
            }.ToQueryString()}");

        var result = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();
        var content =
            JsonConvert.DeserializeObject<ListWithPaginationData<CourseSummary>>(
                await response.Content.ReadAsStringAsync());
        content!.Data.Count.Should().Be(2);
        content.Data[0].ScoredPoints.Should().Be(5);
        content.Data[0].MaxPoints.Should().Be(35);
        content.Data[1].ScoredPoints.Should().Be(0);
    }

    [Fact]
    public async Task Delete_ShouldDelete_WhenCourseExist()
    {
        _client.Login(UserRole.Admin);
        
        var response = await _client.DeleteAsync("api/Courses/210");

        response.EnsureSuccessStatusCode();
        await using var context = ApplicationFactory.Context;
        (await context.Courses.AnyAsync(course => course.Id == 210)).Should().BeFalse();
    }

    private static async Task SeedData()
    {
        await using var context = ApplicationFactory.Context;
        var groups = new List<Group>
        {
            new()
            {
                Id = 200, Name = "g1Name", DepartmentId = 1, Students = new List<Student>
                {
                    new() { Id = 200, FirstName = "s1fn", LastName = "s1ln", Submissions = CreateSubmission(200) },
                    new() { Id = 201, FirstName = "s2fn", LastName = "s2ln", Submissions = CreateSubmission(204) }
                }
            },
            new()
            {
                Id = 201, Name = "g2Name", DepartmentId = 1, Students = new List<Student>
                {
                    new() { Id = 203, FirstName = "s4fn", LastName = "s4ln", Submissions = CreateSubmission(208) },
                    new() { Id = 204, FirstName = "s5fn", LastName = "s5ln", Submissions = CreateSubmission(212) }
                }
            }
        };
        var teachers = new List<Teacher>
        {
            new() { Id = 200, FirstName = "t1fn", LastName = "t2fn", DepartmentId = 1 }
        };
        var course = new Course
        {
            Id = 200, SubjectId = 1, Name = "c1Name", Groups = groups,
            Tasks = new List<CourseTask>
            {
                new() { Id = 200, Name = "ct1Name", MaxPoints = 10 },
                new() { Id = 201, Name = "ct2Name", MaxPoints = 10 },
                new() { Id = 202, Name = "ct3Name", MaxPoints = 10, Coefficient = 0.5 },
                new() { Id = 203, Name = "ct4Name", MaxPoints = 10 }
            },
            Teachers = teachers
        };
        var emptyCourse = new Course
        {
            Id = 201, SubjectId = 1, Name = "emptyCourse",
            CourseTeachers = new List<CourseTeacher> { new() { TeacherId = 200 } },
            CourseGroups = new List<CourseGroup> { new() { GroupId = 200 } }
        };
        context.Add(course);
        context.Add(emptyCourse);
        context.Add(new Course { Id = 210, SubjectId = 1, Name = "courseForDelete" });
        await context.SaveChangesAsync();
    }

    private static List<TaskSubmission> CreateSubmission(int firstItemId) =>
        Enumerable.Range(0, 4).Select(i => new TaskSubmission
            { Id = i + firstItemId, CourseTaskId = 200 + i % 4, TotalPoints = i }).ToList();
}