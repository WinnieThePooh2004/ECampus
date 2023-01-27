using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ECampus.Api.MiddlewareFilters;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using ECampus.Tests.Shared.Mocks.HttpRequests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Integration.Tests.AuditoriesEndpoints;

public class AuditoriesSuccessfulEndpointsTests : IClassFixture<ApplicationFactory>, IAsyncLifetime
{
    private static bool _databaseCreated;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions = HttpClientFactory.Options;

    public AuditoriesSuccessfulEndpointsTests(ApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _client.Login(DefaultUsers.GetUserByRole(UserRole.Admin));
    }

    public async Task InitializeAsync()
    {
        if (_databaseCreated)
        {
            return;
        }

        _databaseCreated = true;
        await CreateTestData();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Get_ShouldReturnTeacherAndSubjects_IfExists()
    {
        var response = await _client.GetAsync($"/api/Auditories/2");
        response.EnsureSuccessStatusCode();
        var auditory =
            JsonSerializer.Deserialize<Auditory>(await response.Content.ReadAsStringAsync(), _serializerOptions);
        auditory.Should().NotBeNull();
        auditory?.Id.Should().Be(2);
        auditory?.Name.Should().Be("name2");
        auditory?.Building.Should().Be("building2");
    }

    [Fact]
    public async Task Update_ShouldUpdateInDb()
    {
        var teacher = new AuditoryDto
        {
            Id = 1,
            Name = "newName",
            Building = "NewBuilding"
        };
        var response = await _client.PutAsJsonAsync("/api/Auditories", teacher);
        response.EnsureSuccessStatusCode();
        var context = ApplicationFactory.Context;
        var objectInDb = await context.FindAsync<Auditory>(1);
        objectInDb.Should().BeEquivalentTo(teacher, options => options.ComparingByMembers<AuditoryDto>());
    }

    [Fact]
    public async Task Create_ShouldAddToDataBase_WhenNoValidationErrorOccured()
    {
        var teacher = new AuditoryDto
        {
            Id = 40,
            Name = "name40",
            Building = "building40"
        };
        var response = await _client.PostAsJsonAsync("/api/Auditories", teacher);
        response.EnsureSuccessStatusCode();
        (await ApplicationFactory.Context.Auditories.FindAsync(40)).Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_ShouldReturnDeleteInDb()
    {
        var response = await _client.DeleteAsync("/api/Auditories/3");
        response.EnsureSuccessStatusCode();
        (await ApplicationFactory.Context
            .Auditories.SingleOrDefaultAsync(t => t.Id == 3)).Should().BeNull();
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
    
    private static async Task CreateTestData()
    {
        await using var context = ApplicationFactory.Context;
        context.AddRange
        (
            new Auditory
            {
                Id = 1,
                Name = "name1",
                Building = "building1"
            },
            new Auditory
            {
                Id = 2,
                Name = "name2",
                Building = "building2"
            },
            new Auditory
            {
                Id = 3,
                Name = "name3",
                Building = "building3"
            }
        );
        await context.SaveChangesAsync();
    }
}