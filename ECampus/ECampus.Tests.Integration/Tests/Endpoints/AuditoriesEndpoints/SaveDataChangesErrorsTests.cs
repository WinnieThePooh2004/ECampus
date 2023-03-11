using System.Net;
using ECampus.Domain.Entities;
using ECampus.Domain.Enums;
using ECampus.Domain.Exceptions.InfrastructureExceptions;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using ECampus.WebApi.MiddlewareFilters;
using FluentAssertions;
using Newtonsoft.Json;

namespace ECampus.Tests.Integration.Tests.Endpoints.AuditoriesEndpoints;

public class SaveDataChangesErrorsTests : IClassFixture<ApplicationFactory>
{
    private readonly HttpClient _client;

    public SaveDataChangesErrorsTests(ApplicationFactory app)
    {
        _client = app.CreateClient();
        _client.Login(DefaultUsers.GetUserByRole(UserRole.Admin));
    }

    [Fact]
    public async Task GetById_ShouldReturn404_IfNotExist()
    {
        var response = await _client.GetAsync("/api/Auditories/-1");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var result = JsonConvert
            .DeserializeObject<BadResponseObject>(await response.Content.ReadAsStringAsync());
        result.Should().NotBeNull();
        result?.Message.Should().Be(new ObjectNotFoundByIdException(typeof(Auditory), -1).Message);
    }

    [Fact]
    public async Task Delete_ShouldReturn404_WhenNotExist()
    {
        var response = await _client.GetAsync("/api/Auditories/-1");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var result = JsonConvert
            .DeserializeObject<BadResponseObject>(await response.Content.ReadAsStringAsync());
        result.Should().NotBeNull();
        result?.Message.Should().Be(new ObjectNotFoundByIdException(typeof(Auditory), -1).Message);
    }
}