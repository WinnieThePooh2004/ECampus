using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Infrastructure;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models.RelationModels;
using UniversityTimetable.Tests.Integration.AppFactories;
using UniversityTimetable.Tests.Integration.AuthHelpers;

namespace UniversityTimetable.Tests.Integration.Tests.PutTests;

public class PutTests : IClassFixture<ApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly ApplicationDbContext _context;

    public PutTests(ApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _context = factory.Context;
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
    public async Task UpdateTeacher_ShouldUpdateRelationModels()
    {
        _context.ChangeTracker.Clear();
        var teacher = new TeacherDto
        {
            Id = 1,
            LastName = "ln1",
            FirstName = "fn1",
            Subjects = new List<SubjectDto> { new() { Id = 1 }, new() { Id = 3 } }
        };
        var response = await _client.PutAsJsonAsync("api/Teachers", teacher);
        var context = await response.Content.ReadAsStringAsync(); 
        response.EnsureSuccessStatusCode();
        (await _context.SubjectTeachers.ToListAsync()).Should().BeEquivalentTo(new List<SubjectTeacher>
            { new() { SubjectId = 1, TeacherId = 1 }, new() { SubjectId = 3, TeacherId = 1 } });
    }
}