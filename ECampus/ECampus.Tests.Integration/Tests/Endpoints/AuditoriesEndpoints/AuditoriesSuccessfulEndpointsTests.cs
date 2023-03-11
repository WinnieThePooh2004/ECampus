using System.Net;
using System.Net.Http.Json;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Domain.Models;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using ECampus.WebApi.MiddlewareFilters;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ECampus.Tests.Integration.Tests.Endpoints.AuditoriesEndpoints;

public class AuditoriesSuccessfulEndpointsTests : IClassFixture<ApplicationFactory>, IAsyncLifetime
{
    private static bool _databaseCreated;
    private readonly HttpClient _client;

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
        var response = await _client.GetAsync("/api/Auditories/101");
        response.EnsureSuccessStatusCode();
        var auditory =
            JsonConvert.DeserializeObject<Auditory>(await response.Content.ReadAsStringAsync());
        auditory.Should().NotBeNull();
        auditory?.Id.Should().Be(101);
        auditory?.Name.Should().Be("name2");
        auditory?.Building.Should().Be("building2");
    }

    [Fact]
    public async Task Update_ShouldUpdateInDb()
    {
        var teacher = new AuditoryDto
        {
            Id = 100,
            Name = "newName",
            Building = "NewBuilding"
        };
        var response = await _client.PutAsJsonAsync("/api/Auditories", teacher);
        response.EnsureSuccessStatusCode();
        var context = ApplicationFactory.Context;
        var objectInDb = await context.FindAsync<Auditory>(100);
        objectInDb.Should().BeEquivalentTo(teacher, options => options.ComparingByMembers<AuditoryDto>());
    }

    [Fact]
    public async Task Create_ShouldAddToDataBase_WhenNoValidationErrorOccured()
    {
        var teacher = new AuditoryDto
        {
            Id = 104,
            Name = "name40",
            Building = "building40"
        };
        var response = await _client.PostAsJsonAsync("/api/Auditories", teacher);
        response.EnsureSuccessStatusCode();
        (await ApplicationFactory.Context.Auditories.FindAsync(104)).Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_ShouldReturnDeleteInDb()
    {
        var response = await _client.DeleteAsync("/api/Auditories/102");
        response.EnsureSuccessStatusCode();
        (await ApplicationFactory.Context
            .Auditories.SingleOrDefaultAsync(t => t.Id == 102)).Should().BeNull();
    }

    [Fact]
    public async Task Delete_ShouldReturn404_WhenNoObjectExists()
    {
        var response = await _client.DeleteAsync("/api/Auditories/-1");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var result = JsonConvert.DeserializeObject<BadResponseObject>(await response.Content.ReadAsStringAsync());
        result.Should().NotBeNull();
    }

    private static async Task CreateTestData()
    {
        await using var context = ApplicationFactory.Context;
        context.AddRange
        (
            new Auditory
            {
                Id = 100,
                Name = "name1",
                Building = "building1"
            },
            new Auditory
            {
                Id = 101,
                Name = "name2",
                Building = "building2"
            },
            new Auditory
            {
                Id = 102,
                Name = "name3",
                Building = "building3"
            }
        );
        await context.SaveChangesAsync();
    }
}