using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using FluentAssertions;

namespace ECampus.Tests.Integration.Tests.Endpoints.TaskSubmissionEndpoints;

public class AccessRulesFailsTests : IClassFixture<ApplicationFactory>
{
    private readonly HttpClient _client;

    public AccessRulesFailsTests(ApplicationFactory app)
    {
        _client = app.CreateClient();
    }

    [Fact]
    public async Task UpdateContent_ShouldReturn403_WhenSenderIsNotStudent()
    {
        _client.Login(UserRole.Teacher);

        var response = await _client.PutAsJsonAsync($"/api/TaskSubmissions/content",
            JsonSerializer.Serialize(new UpdateSubmissionContentDto()));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}