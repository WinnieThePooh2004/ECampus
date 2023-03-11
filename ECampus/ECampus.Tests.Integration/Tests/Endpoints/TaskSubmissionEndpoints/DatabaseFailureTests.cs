using System.Net;
using ECampus.Domain.Entities;
using ECampus.Domain.Enums;
using ECampus.Domain.Exceptions.InfrastructureExceptions;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using ECampus.WebApi.MiddlewareFilters;
using FluentAssertions;
using Newtonsoft.Json;

namespace ECampus.Tests.Integration.Tests.Endpoints.TaskSubmissionEndpoints;

public class DatabaseFailureTests : IClassFixture<ApplicationFactory>
{
    private readonly HttpClient _client;

    public DatabaseFailureTests(ApplicationFactory app)
    {
        _client = app.CreateClient();
    }

    [Fact]
    public async Task GetById_ShouldReturn404_WhenNoObjectFoundInDb()
    {
        _client.Login(UserRole.Teacher);
        
        var response = await _client.GetAsync("/api/TaskSubmissions/-1");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var result = JsonConvert
            .DeserializeObject<BadResponseObject>(await response.Content.ReadAsStringAsync());
        result.Should().NotBeNull();
        result?.Message.Should().Be(new ObjectNotFoundByIdException(typeof(TaskSubmission), -1).Message);
    }
}
