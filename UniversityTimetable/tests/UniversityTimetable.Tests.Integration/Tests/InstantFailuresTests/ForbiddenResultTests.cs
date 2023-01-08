using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Integration.AppFactories;
using UniversityTimetable.Tests.Integration.AuthHelpers;
using UniversityTimetable.Tests.Shared.Mocks.EntityFramework;

namespace UniversityTimetable.Tests.Integration.Tests.InstantFailuresTests;

public class ForbiddenResultTests : IClassFixture<ApplicationWithoutDatabase>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly ApplicationWithoutDatabase _applicationFactory;

    public ForbiddenResultTests(ApplicationWithoutDatabase applicationFactory)
    {
        _applicationFactory = applicationFactory;
        _client = _applicationFactory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        var user = DefaultUsers.Guest;
        _applicationFactory.Context.Users = new DbSetMock<User>(user);
        await _client.Login(user);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task PutToAuditories_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.PutAsJsonAsync("api/Auditories", new AuditoryDto());
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PostToAuditories_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("api/Auditories", new AuditoryDto());
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteFromAuditories_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Auditories/{100}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PutToClasses_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.PutAsJsonAsync("api/Timetable", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PostToClasses_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("api/Timetable", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteFromClasses_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Timetable/{100}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PutToDepartments_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.PutAsJsonAsync("api/Departments", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PostToDepartments_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("api/Departments", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteFromDepartments_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Departments/{100}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PutToFaculties_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.PutAsJsonAsync("api/Faculties", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PostToFaculties_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("api/Faculties", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteFromFaculties_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Faculties/{100}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PutToGroups_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.PutAsJsonAsync("api/Groups", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PostToGroups_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("api/Groups", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteFromGroups_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Groups/{100}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PutToTeachers_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.PutAsJsonAsync("api/Teachers", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PostToTeachers_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("api/Teachers", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteFromTeachers_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Teachers/{100}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PutToSubjects_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.PutAsJsonAsync("api/Subjects", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PostToSubjects_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("api/Subjects", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteFromSubjects_ShouldReturn403_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Subjects/{100}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}