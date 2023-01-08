using System.Text.Json;
using FluentAssertions;
using UniversityTimetable.Infrastructure;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Tests.Integration.AppFactories;
using UniversityTimetable.Tests.Integration.AuthHelpers;

namespace UniversityTimetable.Tests.Integration.Tests.GetTests;

public class SingleItemGetTests : IClassFixture<ApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions;

    public SingleItemGetTests(ApplicationFactory applicationFactory)
    {
        _client = applicationFactory.CreateClient();
        _serializerOptions = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true };
    }

    public async Task InitializeAsync()
    {
        await _client.Login(DefaultUsers.Admin);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetTeacher_ShouldReturnTeacherAndSubjects_IfTeacherExists()
    {
        var response = await _client.GetAsync("api/Teachers/1");
        response.EnsureSuccessStatusCode();
        var teacher = JsonSerializer.Deserialize<TeacherDto>(await response.Content.ReadAsStringAsync(), _serializerOptions);
        teacher.Should().NotBeNull();
        teacher?.Subjects.Should().NotBeNull();
        teacher?.Subjects?.Count.Should().Be(2);
    }
}