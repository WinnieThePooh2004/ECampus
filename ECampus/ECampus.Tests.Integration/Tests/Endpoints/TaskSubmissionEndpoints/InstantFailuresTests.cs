using System.Net;
using System.Net.Http.Json;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using FluentAssertions;
using Newtonsoft.Json;

namespace ECampus.Tests.Integration.Tests.Endpoints.TaskSubmissionEndpoints;

public class InstantFailuresTests : IClassFixture<ApplicationWithoutDatabase>
{
    private readonly HttpClient _client;

    public InstantFailuresTests(ApplicationWithoutDatabase app)
    {
        _client = app.CreateClient();
    }

    // [Fact]
    // public async Task UpdateContent_ShouldReturn400_WhenContentToBig()
    // {
    //     _client.Login(DefaultUsers.GetUserByRole(UserRole.Student));
    //     var response = await _client.PutAsJsonAsync("/api/TaskSubmissions/content/",
    //         JsonConvert.SerializeObject(new UpdateSubmissionContentDto
    //             { SubmissionId = 10, Content = CreateLargeString() }));
    //     response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    //     var responseObject =
    //         JsonConvert.DeserializeObject<BadResponseObject>(await response.Content.ReadAsStringAsync());
    //     var expectedErrors = new List<ValidationError>
    //     {
    //         new(nameof(TaskSubmissionDto.SubmissionContent), "Submission must contain not more than 450 symbols")
    //     };
    //
    //     responseObject.Should().NotBeNull();
    //     responseObject!.Message.Should().BeEquivalentTo(new ValidationException(typeof(TaskSubmissionDto),
    //         new ValidationResult(expectedErrors)).Message);
    //     var actualErrors = JsonConvert.DeserializeObject<ValidationResult>(responseObject.ResponseObject!.ToString()!);
    //     actualErrors!.ToList().Should().BeEquivalentTo(expectedErrors);
    // }

    [Fact]
    public async Task UpdateContent_ShouldReturn403_WhenUserIsNotStudent()
    {
        _client.Login(DefaultUsers.GetUserByRole(UserRole.Admin));
        var response = await _client.PutAsJsonAsync("/api/TaskSubmissions/content/",
            JsonConvert.SerializeObject(new UpdateSubmissionContentDto { SubmissionId = 10, Content = "28" }));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}