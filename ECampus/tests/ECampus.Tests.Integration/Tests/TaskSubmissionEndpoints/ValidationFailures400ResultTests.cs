using System.Net;
using System.Net.Http.Json;
using ECampus.Api.MiddlewareFilters;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using FluentAssertions;
using Newtonsoft.Json;

namespace ECampus.Tests.Integration.Tests.TaskSubmissionEndpoints;

public class ValidationFailures400ResultTests : IClassFixture<ApplicationWithoutDatabase>
{
    private readonly HttpClient _client;

    public ValidationFailures400ResultTests(ApplicationWithoutDatabase app)
    {
        _client = app.CreateClient();
        _client.Login(DefaultUsers.Admin);
    }

    [Fact]
    public async Task Create_ShouldReturn400_Always()
    {
        var submission = new TaskSubmissionDto();
        var response = await _client.PostAsJsonAsync("/api/TaskSubmissions", submission);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseObject =
            JsonConvert.DeserializeObject<BadResponseObject>(await response.Content.ReadAsStringAsync());
        var expectedErrors = new List<ValidationError>
        {
            new(nameof(TaskSubmissionDto.Id),
                $"Objects of type {typeof(TaskSubmission)} can be created" +
                $" only during creating new objects of type {typeof(CourseTaskDto)}")
        };

        responseObject.Should().NotBeNull();
        responseObject!.Message.Should().BeEquivalentTo(new ValidationException(typeof(TaskSubmissionDto),
            new ValidationResult(expectedErrors)).Message);
        var actualErrors = JsonConvert.DeserializeObject<ValidationResult>(responseObject.ResponseObject!.ToString()!);
        actualErrors!.GetAllErrors().Should().BeEquivalentTo(expectedErrors);
    }
}