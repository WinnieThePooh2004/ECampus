using System.Net;
using System.Net.Http.Json;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Domain.Models;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using FluentAssertions;

namespace ECampus.Tests.Integration.Tests.InstantFailuresTests;

public class ForbiddenResultTests : IClassFixture<ApplicationWithoutDatabase>
{
    private readonly HttpClient _client;

    public ForbiddenResultTests(ApplicationWithoutDatabase applicationFactory)
    {
        _client = applicationFactory.CreateClient();
        var user = DefaultUsers.GetUserByRole(UserRole.Guest);
        applicationFactory.Context.Users = new DbSetMock<User>(user);
        _client.Login(user);
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