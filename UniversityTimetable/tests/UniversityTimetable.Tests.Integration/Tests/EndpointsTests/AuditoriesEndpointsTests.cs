using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Api.MiddlewareFilters;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;
using UniversityTimetable.Tests.Integration.AppFactories;
using UniversityTimetable.Tests.Integration.AuthHelpers;
using UniversityTimetable.Tests.Shared.Mocks.HttpRequests;

namespace UniversityTimetable.Tests.Integration.Tests.EndpointsTests;

public class AuditoriesEndpointsTests : IClassFixture<ApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions = HttpClientFactory.Options;

    public AuditoriesEndpointsTests(ApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _client.Login(DefaultUsers.Admin);
    }

    public async Task InitializeAsync()
    {
        var context = ApplicationFactory.Context;
        await context.Database.EnsureCreatedAsync();
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

    public async Task DisposeAsync()
    {
        await ApplicationFactory.Context.Database.EnsureDeletedAsync();
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

    [Fact]
    public async Task Get_ShouldReturnTeacherAndSubjects_IfExists()
    {
        var response = await _client.GetAsync($"/api/Auditories/{1}");
        response.EnsureSuccessStatusCode();
        var auditory =
            JsonSerializer.Deserialize<Auditory>(await response.Content.ReadAsStringAsync(), _serializerOptions);
        auditory.Should().NotBeNull();
        auditory?.Id.Should().Be(1);
        auditory?.Name.Should().Be("name1");
        auditory?.Building.Should().Be("building1");
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
    public async Task CreateTeacher_ShouldAddToDataBase_WhenNoValidationErrorOccured()
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
    public async Task CreateTeacher_ShouldReturn400_WhenValidationErrorOccured()
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

    [Fact]
    public async Task DeleteTeacher_ShouldReturnDeleteInDb()
    {
        var response = await _client.DeleteAsync("/api/Auditories/1");
        response.EnsureSuccessStatusCode();
        (await ApplicationFactory.Context.Auditories.CountAsync()).Should().Be(2);
    }

    [Fact]
    public async Task DeleteTeacher_ShouldReturn404_WhenNoObjectExists()
    {
        var response = await _client.DeleteAsync("/api/Auditories/10");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var result = JsonSerializer
            .Deserialize<BadResponseObject>(await response.Content.ReadAsStringAsync(), _serializerOptions);
        result.Should().NotBeNull();
        result?.Message.Should().Be(new ObjectNotFoundByIdException(typeof(Auditory), 10).Message);
    }
}