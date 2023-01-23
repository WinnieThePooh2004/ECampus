using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ECampus.Api.MiddlewareFilters;
using ECampus.Shared.DataTransferObjects;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using ECampus.Tests.Shared.Mocks.HttpRequests;
using FluentAssertions;

namespace ECampus.Tests.Integration.Tests.AuditoriesEndpoints;

public class AuditoriesValidationFailuresTests : IClassFixture<ApplicationWithoutDatabase>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions = HttpClientFactory.Options;

    public AuditoriesValidationFailuresTests(ApplicationWithoutDatabase app)
    {
        _client = app.CreateClient();
        _client.Login(DefaultUsers.Admin);
    }
    
    [Fact]
    public async Task Update_ShouldReturn400_WhenValidationErrorOccured()
    {
        var teacher = new AuditoryDto { Id = 1 };
        var response = await _client.PutAsJsonAsync("/api/Auditories", teacher);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = JsonSerializer
            .Deserialize<BadResponseObject>(await response.Content.ReadAsStreamAsync(), _serializerOptions);
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
        var result = JsonSerializer
            .Deserialize<BadResponseObject>(await response.Content.ReadAsStreamAsync(), _serializerOptions);
        response.Should().NotBeNull();
        result?.Message.Should()
            .Be($"2 errors occured while validating entity of type {typeof(AuditoryDto)}\nError code: 400");
    }
}