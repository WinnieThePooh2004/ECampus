using System.Net;
using FluentAssertions;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Integration.AppFactories;
using UniversityTimetable.Tests.Integration.AuthHelpers;
using UniversityTimetable.Tests.Shared.Mocks.EntityFramework;

namespace UniversityTimetable.Tests.Integration.Tests.InstantFailuresTests;

public class BadRequestStatusCodeWhenIdIsNullTests : IClassFixture<ApplicationFactory>, IAsyncLifetime
{
    private readonly ApplicationFactory _factory;
    private readonly HttpClient _client;

    public BadRequestStatusCodeWhenIdIsNullTests(ApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }


    public async Task InitializeAsync()
    {
        var user = DefaultUsers.Admin;
        _factory.Context.Users = new DbSetMock<User>(user);
        await _client.Login(user);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task DeleteFromAuditories_ShouldReturn400_WhenNoIdPassed()
    {
        var response = await _client.DeleteAsync("api/Auditories");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task DeleteFromClasses_ShouldReturn400_WhenNoIdPassed()
    {
        var response = await _client.DeleteAsync("api/Timetable");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task DeleteFromDepartments_ShouldReturn400_WhenNoIdPassed()
    {
        var response = await _client.DeleteAsync("api/Departments");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task DeleteFromFaculties_ShouldReturn400_WhenNoIdPassed()
    {
        var response = await _client.DeleteAsync("api/Faculties");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    
    [Fact]
    public async Task DeleteFromGroups_ShouldReturn400_WhenNoIdPassed()
    {
        var response = await _client.DeleteAsync("api/Groups");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task DeleteFromSubject_ShouldReturn400_WhenNoIdPassed()
    {
        var response = await _client.DeleteAsync("api/Subjects");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task DeleteFromTeachers_ShouldReturn400_WhenNoIdPassed()
    {
        var response = await _client.DeleteAsync("api/Teachers");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task DeleteFromUsers_ShouldReturn400_WhenNoIdPassed()
    {
        var response = await _client.DeleteAsync("api/Users");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetUser_ShouldReturn400_WhenNoIdPassed()
    {
        var response = await _client.GetAsync("api/Users");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetClass_ShouldReturn400_WhenNoIdPassed()
    {
        var response = await _client.GetAsync("api/Timetable");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task TimetableForAuditory_ShouldReturn400_WhenNoIdPasses()
    {
        var response = await _client.GetAsync("api/Timetable/Auditory");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task TimetableForGroup_ShouldReturn400_WhenNoIdPasses()
    {
        var response = await _client.GetAsync("api/Timetable/Group");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task TimetableForTeacher_ShouldReturn400_WhenNoIdPasses()
    {
        var response = await _client.GetAsync("api/Timetable/Teacher");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}