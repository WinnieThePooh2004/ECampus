using System.Net;
using System.Net.Http.Json;
using ECampus.Api.MiddlewareFilters;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Exceptions.DomainExceptions;
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
    }

    [Fact]
    public async Task UpdateContent_ShouldReturn400_WhenContentToBig()
    {
        _client.Login(DefaultUsers.GetUserByRole(UserRole.Student));
        var response = await _client.PutAsJsonAsync("/api/TaskSubmissions/content/10", CreateLargeString());
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseObject =
            JsonConvert.DeserializeObject<BadResponseObject>(await response.Content.ReadAsStringAsync());
        var expectedErrors = new List<ValidationError>
        {
            new(nameof(TaskSubmissionDto.SubmissionContent), "Submission must contain not more than 450 symbols")
        };

        responseObject.Should().NotBeNull();
        responseObject!.Message.Should().BeEquivalentTo(new ValidationException(typeof(TaskSubmissionDto),
            new ValidationResult(expectedErrors)).Message);
        var actualErrors = JsonConvert.DeserializeObject<ValidationResult>(responseObject.ResponseObject!.ToString()!);
        actualErrors!.ToList().Should().BeEquivalentTo(expectedErrors);
    }

    [Fact]
    public async Task UpdateContent_ShouldReturn403_WhenUserIsNotStudent()
    {
        _client.Login(DefaultUsers.GetUserByRole(UserRole.Admin));
        var response = await _client.PutAsJsonAsync("/api/TaskSubmissions/content/10", "very small content");
        
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private string CreateLargeString()
    {
        var result = new char[500];
        result[0] = 'a';
        result[499] = 'a';
        return new string(result);
    }
}