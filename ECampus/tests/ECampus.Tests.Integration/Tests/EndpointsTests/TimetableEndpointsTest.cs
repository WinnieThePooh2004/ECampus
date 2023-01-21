using System.Net;
using System.Text.Json;
using ECampus.Api.MiddlewareFilters;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using ECampus.Tests.Shared.Mocks.HttpRequests;
using FluentAssertions;

namespace ECampus.Tests.Integration.Tests.EndpointsTests;

public class TimetableEndpointsTest : IClassFixture<ApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions = HttpClientFactory.Options;

    public TimetableEndpointsTest(ApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _client.Login(DefaultUsers.Admin);
    }

    public async Task InitializeAsync()
    {
        await CreateTestsData();
    }

    public async Task DisposeAsync()
    {
        await ApplicationFactory.Context.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task GetById_ShouldReturn404_IfClassNotExist()
    {
        var response = await _client.GetAsync($"/api/Timetable/{10}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var result = JsonSerializer
            .Deserialize<BadResponseObject>(await response.Content.ReadAsStringAsync(), _serializerOptions);
        result.Should().NotBeNull();
        result?.Message.Should().Be(new ObjectNotFoundByIdException(typeof(Class), 10).Message);
    }

    private static async Task CreateTestsData()
    {
        await using var context = ApplicationFactory.Context;
        await context.Database.EnsureCreatedAsync();
        context.Add(new Teacher { Id = 20 });
        context.Add(new Subject { Id = 20 });
        context.Add(new Group { Id = 20 });
        context.Add(new Auditory { Id = 20 });
        context.Add(new Class { Id = 1, AuditoryId = 20, GroupId = 20, TeacherId = 20, SubjectId = 20 });
        await context.SaveChangesAsync();
    }
}