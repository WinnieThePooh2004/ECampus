using System.Net;
using System.Net.Http.Json;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Enums;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using FluentAssertions;

namespace ECampus.Tests.Integration.Tests.Endpoints.TaskSubmissionEndpoints;

public class SuccessfulEndpointsTests : IClassFixture<ApplicationFactory>, IAsyncLifetime
{
    private static bool _databaseCreated;
    private readonly HttpClient _client;

    public SuccessfulEndpointsTests(ApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        if (_databaseCreated)
        {
            return;
        }

        _databaseCreated = true;
        await SeedData();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task UpdateContent_ShouldSendEmail_WhenContentUpdated()
    {
        var user = DefaultUsers.GetUserByRole(UserRole.Student);
        user.StudentId = 600;
        _client.Login(user);
        const string content = "new content";

        var response = await _client.PutAsJsonAsync("/api/TaskSubmissions/content",
            new UpdateSubmissionContentDto { SubmissionId = 600, Content = content });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await using var context = ApplicationFactory.Context;
        var submission = await context.FindAsync<TaskSubmission>(600);
        submission!.SubmissionContent.Should().Be(content);
        //await ApplicationFactory.AmazonSnsMock.Received().PublishAsync(Arg.Any<PublishRequest>());
    }

    [Fact]
    public async Task UpdateMark_ShouldSendEmail_WhenContentUpdated()
    {
        var user = DefaultUsers.GetUserByRole(UserRole.Teacher);
        user.TeacherId = 600;
        _client.Login(user);

        var response = await _client.PutAsJsonAsync("/api/TaskSubmissions/mark",
            new UpdateSubmissionMarkDto { SubmissionId = 600, Mark = 10 });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await using var context = ApplicationFactory.Context;
        var submission = await context.FindAsync<TaskSubmission>(600);
        submission!.TotalPoints.Should().Be(10);
        //await ApplicationFactory.AmazonSnsMock.Received().PublishAsync(Arg.Any<PublishRequest>());
    }

    private static async Task SeedData()
    {
        var course = new Course
        {
            Id = 600, SubjectId = 1, Name = "c1Name",
            Teachers =
                new List<Teacher> { new() { Id = 600, FirstName = "t1fn", LastName = "t1ln", DepartmentId = 1 } },
            Groups = new List<Group> { new() { Id = 600, DepartmentId = 1, Name = "g1Name" } }
        };
        var task = new CourseTask
        {
            Id = 600, CourseId = 600, Name = "ct1Name", MaxPoints = 10,
            Submissions = Enumerable.Range(0, 10).Select(i => new TaskSubmission
            {
                Id = 600 + i, SubmissionContent = "", Student = new Student
                {
                    Id = 600 + i, GroupId = 600, FirstName = $"s{i}fn", LastName = $"s{i}ln"
                }
            }).ToList()
        };

        await using var context = ApplicationFactory.Context;
        context.Add(course);
        context.Add(task);
        await context.SaveChangesAsync();
    }
}