using System.Net;
using System.Text.Json;
using ECampus.Domain.Enums;
using ECampus.Domain.Exceptions.InfrastructureExceptions;
using ECampus.Domain.Models;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using ECampus.Tests.Shared.Mocks.HttpRequests;
using ECampus.WebApi.MiddlewareFilters;
using FluentAssertions;

namespace ECampus.Tests.Integration.Tests.Endpoints.TimetableEndpoints;

public class SuccessfulTimetableEndpointsTest : IClassFixture<ApplicationFactory>, IAsyncLifetime
{
    private static bool _dbCreated;
    
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions = HttpClientFactory.Options;

    public SuccessfulTimetableEndpointsTest(ApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _client.Login(DefaultUsers.GetUserByRole(UserRole.Admin));
    }

    public async Task InitializeAsync()
    {
        if (_dbCreated)
        {
            return;
        }

        _dbCreated = true;
        await CreateTestsData();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetById_ShouldReturn404_IfClassNotExist()
    {
        var response = await _client.GetAsync($"/api/Timetable/-1");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var result = JsonSerializer
            .Deserialize<BadResponseObject>(await response.Content.ReadAsStringAsync(), _serializerOptions);
        result.Should().NotBeNull();
        result?.Message.Should().Be(new ObjectNotFoundByIdException(typeof(Class), -1).Message);
    }

    [Fact]
    public async Task DeleteShouldReturn404_WhenClassNotExist()
    {
        var response = await _client.DeleteAsync("/api/Timetable/-1");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static async Task CreateTestsData()
    {
        await using var context = ApplicationFactory.Context;
        context.Add(new Teacher { Id = 400, DepartmentId = 1 });
        context.Add(new Subject { Id = 400 });
        context.Add(new Group { Id = 400, DepartmentId = 1 });
        context.Add(new Auditory { Id = 400 });
        context.Add(new Class { Id = 400, AuditoryId = 400, GroupId = 400, TeacherId = 400, SubjectId = 400 });
        await context.SaveChangesAsync();
    }
}