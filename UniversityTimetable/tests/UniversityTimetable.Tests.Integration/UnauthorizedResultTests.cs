using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Tests.Integration;

public class UnauthorizedResultTests : IClassFixture<ApplicationFactory>
{
    private readonly HttpClient _client;
    
    public UnauthorizedResultTests(ApplicationFactory factory)
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
}