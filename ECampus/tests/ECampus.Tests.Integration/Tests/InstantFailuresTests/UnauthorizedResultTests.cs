using System.Net;
using System.Net.Http.Json;
using ECampus.Shared.DataTransferObjects;
using ECampus.Tests.Integration.AppFactories;
using FluentAssertions;

namespace ECampus.Tests.Integration.Tests.InstantFailuresTests;

public class UnauthorizedResultTests : IClassFixture<ApplicationWithoutDatabase>
{
    private readonly HttpClient _client;

    public UnauthorizedResultTests(ApplicationWithoutDatabase factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PutToAuditories_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.PutAsJsonAsync("api/Auditories", new AuditoryDto());
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PostToAuditories_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("api/Auditories", new AuditoryDto());
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteFromAuditories_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Auditories/{100}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PutToClasses_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.PutAsJsonAsync("api/Timetable", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PostToClasses_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("api/Timetable", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteFromClasses_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Timetable/{100}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task PutToDepartments_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.PutAsJsonAsync("api/Departments", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PostToDepartments_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("api/Departments", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteFromDepartments_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Departments/{100}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task PutToFaculties_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.PutAsJsonAsync("api/Faculties", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PostToFaculties_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("api/Faculties", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteFromFaculties_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Faculties/{100}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task PutToGroups_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.PutAsJsonAsync("api/Groups", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PostToGroups_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("api/Groups", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteFromGroups_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Groups/{100}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task PutToTeachers_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.PutAsJsonAsync("api/Teachers", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PostToTeachers_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("api/Teachers", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteFromTeachers_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Teachers/{100}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task PutToSubjects_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.PutAsJsonAsync("api/Subjects", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PostToSubjects_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("api/Subjects", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteFromSubjects_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Subjects/{100}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task PutToUsers_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.PutAsJsonAsync("api/Users", new ClassDto());
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task DeleteFromUsers_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Users/{100}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task SaveAuditoryUsers_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Users/auditory");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task SaveGroupUsers_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Users/group");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task SaveTeacherUsers_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Users/teacher");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task RemoveSavedAuditoryUsers_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Users/auditory");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task RemoveSavedGroupUsers_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Users/group");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task RemoveSavedTeacherUsers_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.DeleteAsync($"api/Users/teacher");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}