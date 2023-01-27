using System.Net;
using System.Text.Json;
using ECampus.Api.MiddlewareFilters;
using ECampus.Shared.Enums;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using ECampus.Tests.Shared.Mocks.HttpRequests;
using FluentAssertions;

namespace ECampus.Tests.Integration.Tests.AuditoriesEndpoints;

public class Auditories404WhenNoObjectInDb : IClassFixture<ApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions = HttpClientFactory.Options;

    public Auditories404WhenNoObjectInDb(ApplicationFactory app)
    {
        _client = app.CreateClient();
        _client.Login(DefaultUsers.GetUserByRole(UserRole.Admin));
    }

    [Fact]
    public async Task Delete_ShouldReturn404_WhenNoObjectExists()
    {
        var response = await _client.DeleteAsync("/api/Auditories/10");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var result = JsonSerializer
            .Deserialize<BadResponseObject>(await response.Content.ReadAsStringAsync(), _serializerOptions);
        result.Should().NotBeNull();
        result?.Message.Should().Be(new ObjectNotFoundByIdException(typeof(Auditory), 10).Message);
    }

    [Fact]
    public async Task GetById_ShouldReturn404_IfNotExist()
    {
        var response = await _client.GetAsync("/api/Auditories/10");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var result = JsonSerializer
            .Deserialize<BadResponseObject>(await response.Content.ReadAsStringAsync(), _serializerOptions);
        result.Should().NotBeNull();
        result?.Message.Should().Be(new ObjectNotFoundByIdException(typeof(Auditory), 10).Message);
    }
}