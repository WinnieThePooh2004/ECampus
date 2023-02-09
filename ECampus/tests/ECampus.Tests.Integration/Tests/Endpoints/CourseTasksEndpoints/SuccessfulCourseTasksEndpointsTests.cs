using System.Net;
using System.Net.Http.Json;
using ECampus.Shared.Enums;
using ECampus.Shared.Models;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Integration.Tests.Endpoints.CourseTasksEndpoints;

public class SuccessfulCourseTasksEndpointsTests : IClassFixture<ApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private static bool _dataCreated;

    public SuccessfulCourseTasksEndpointsTests(ApplicationFactory app)
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
    public async Task CreateTask_ShouldCreateSubmissions_WhenTaskCreated()
    {
        _client.Login(UserRole.Admin);
        var task = new CourseTask { Id = 503, CourseId = 500, Name = "task3Name" };

        var response = await _client.PostAsJsonAsync("/api/CourseTasks", task);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await using var context = ApplicationFactory.Context;
        (await context.TaskSubmissions.Where(submission => submission.CourseTaskId == 503).ToListAsync()).Should().NotBeEmpty();
    }

    [Fact]
    public async Task Delete_ShouldDelete_WhenTaskExist()
    {
        _client.Login(UserRole.Admin);

        var response = await _client.DeleteAsync("/api/CourseTasks/504");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await using var context = ApplicationFactory.Context;
        (await context.CourseTasks.Where(task => task.Id == 504).CountAsync()).Should().Be(0);
    }

    private static async Task SeedData()
    {
        var course = new Course
        {
            Id = 500, SubjectId = 1, Name = "c1Name",
            Groups = Enumerable.Range(0, 3).Select(i => new Group
            {
                Id = 500 + i, Name = $"g{i}Name", DepartmentId = 1,
                Students = Enumerable.Range(0, 3).Select(j => new Student
                {
                    Id = 500 + j + i * 3, FirstName = $"s{j}fn", LastName = $"s{j}ln",
                    User = new User
                    {
                        Id = 500 + j + i * 3, Email = $"student{j + i * 3}@email.com", Username = $"student{i * 3 + j}"
                    }
                }).ToList()
            }).ToList()
        };
        var courseTasks = Enumerable.Range(0, 2).Select(i => new CourseTask
        {
            Id = 500 + i, Name = "t1Name", CourseId = 500,
            Submissions = Enumerable.Range(0, 9).Select(j => new TaskSubmission
            {
                Id = 500 + j + i * 9, SubmissionContent = "", StudentId = 500 + j
            }).ToList()
        }).ToList();
        var emptyTask = new CourseTask { Id = 504, Name = "task3Name", CourseId = 500 };
        await using var context = ApplicationFactory.Context;
        context.Add(course);
        context.AddRange(courseTasks);
        context.Add(emptyTask);
        await context.SaveChangesAsync();
    }
}