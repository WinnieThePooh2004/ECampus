using System.Net;
using System.Net.Http.Json;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using ECampus.WebApi.MiddlewareFilters;
using FluentAssertions;
using Newtonsoft.Json;

namespace ECampus.Tests.Integration.Tests.Endpoints.AuditoriesEndpoints;

public class InstantFailuresTests : IClassFixture<ApplicationWithoutDatabase>
{
    private readonly HttpClient _client;

    public InstantFailuresTests(ApplicationWithoutDatabase app)
    {
        _client = app.CreateClient();
        _client.Login(DefaultUsers.GetUserByRole(UserRole.Admin));
    }

    [Fact]
    public async Task Update_ShouldReturn400_WhenValidationErrorOccured()
    {
        var teacher = new AuditoryDto { Id = 1 };
        var response = await _client.PutAsJsonAsync("/api/Auditories", teacher);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = JsonConvert.DeserializeObject<BadResponseObject>(await response.Content.ReadAsStringAsync());
        response.Should().NotBeNull();
        result?.Message.Should()
            .Be($"2 errors occured while validating entity of type {typeof(AuditoryDto)}\nError code: 400");
    }

    [Fact]
    public async Task Create_ShouldReturn400_WhenValidationErrorOccured()
    {
        var teacher = new AuditoryDto { Id = 100 };
        var response = await _client.PostAsJsonAsync("/api/Auditories", teacher);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = JsonConvert.DeserializeObject<BadResponseObject>(await response.Content.ReadAsStringAsync());
        response.Should().NotBeNull();
        result?.Message.Should()
            .Be($"2 errors occured while validating entity of type {typeof(AuditoryDto)}\nError code: 400");
    }
}