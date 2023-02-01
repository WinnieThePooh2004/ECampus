using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ECampus.Api.MiddlewareFilters;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using ECampus.Shared.Models.RelationModels;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using ECampus.Tests.Shared.Mocks.HttpRequests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Integration.Tests.Endpoints.TeachersEndpoints;

public class SuccessfulTeachersEndpointsTests : IClassFixture<ApplicationFactory>, IAsyncLifetime
{
    private static bool _databaseCreated;

    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions = HttpClientFactory.Options;

    public SuccessfulTeachersEndpointsTests(ApplicationFactory factory)
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
        await CreateTestsData();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetById_ShouldReturn404_IfTeacherNotExist()
    {
        var response = await _client.GetAsync("/api/Teachers/10");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var result = JsonSerializer
            .Deserialize<BadResponseObject>(await response.Content.ReadAsStringAsync(), _serializerOptions);
        result.Should().NotBeNull();
        result?.Message.Should().Be(new ObjectNotFoundByIdException(typeof(Teacher), 10).Message);
    }

    [Fact]
    public async Task GetTeacher_ShouldReturnTeacherAndSubjects_IfTeacherExists()
    {
        var response = await _client.GetAsync($"/api/Teachers/{1}");
        response.EnsureSuccessStatusCode();
        var teacher =
            JsonSerializer.Deserialize<TeacherDto>(await response.Content.ReadAsStringAsync(), _serializerOptions);
        teacher.Should().NotBeNull();
        teacher?.Subjects.Should().NotBeNull();
        teacher?.Id.Should().Be(1);
    }

    [Fact]
    public async Task UpdateTeacher_ShouldUpdateRelationModels_WhenNoValidationErrorOccured()
    {
        var teacher = new TeacherDto
        {
            Id = 1,
            LastName = "ln1",
            FirstName = "fn1",
            DepartmentId = 1,
            Subjects = new List<SubjectDto> { new() { Id = 1 }, new() { Id = 3 } }
        };
        var response = await _client.PutAsJsonAsync("/api/Teachers", teacher);
        response.EnsureSuccessStatusCode();

        var context = ApplicationFactory.Context;
        var subjectTeachers = await context.SubjectTeachers.ToListAsync();
        subjectTeachers.Should().ContainEquivalentOf(new SubjectTeacher { TeacherId = 1, SubjectId = 3 });
        subjectTeachers.Should().NotContainEquivalentOf(new SubjectTeacher { TeacherId = 1, SubjectId = 2 });
    }

    [Fact]
    public async Task UpdateTeacher_ShouldReturn400_WhenValidationErrorOccured()
    {
        var teacher = new TeacherDto { Id = 1 };
        var response = await _client.PutAsJsonAsync("/api/Teachers", teacher);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result =
            JsonSerializer.Deserialize<BadResponseObject>(await response.Content.ReadAsStreamAsync(),
                _serializerOptions);
        response.Should().NotBeNull();
        result?.Message.Should()
            .Be($"2 errors occured while validating entity of type {typeof(TeacherDto)}\nError code: 400");
    }

    [Fact]
    public async Task CreateTeacher_ShouldAddToDataBase_WhenNoValidationErrorOccured()
    {
        var teacher = new TeacherDto
        {
            Id = 40,
            LastName = "lastname",
            FirstName = "firstName",
            DepartmentId = 1,
            Subjects = new List<SubjectDto>()
        };
        var response = await _client.PostAsJsonAsync("/api/Teachers", teacher);
        response.EnsureSuccessStatusCode();
        (await ApplicationFactory.Context.Teachers.FindAsync(40)).Should().NotBeNull();
    }

    [Fact]
    public async Task CreateTeacher_ShouldReturn400_WhenValidationErrorOccured()
    {
        var teacher = new TeacherDto { Id = 100 };
        var response = await _client.PostAsJsonAsync("/api/Teachers", teacher);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = JsonSerializer
            .Deserialize<BadResponseObject>(await response.Content.ReadAsStreamAsync(), _serializerOptions);
        response.Should().NotBeNull();
        result?.Message.Should()
            .Be($"2 errors occured while validating entity of type {typeof(TeacherDto)}\nError code: 400");
    }

    [Fact]
    public async Task DeleteTeacher_ShouldReturnDeleteInDb()
    {
        var response = await _client.DeleteAsync("/api/Teachers/3");
        response.EnsureSuccessStatusCode();
        (await ApplicationFactory.Context
            .Teachers.SingleOrDefaultAsync(t => t.Id == 3)).Should().BeNull();
    }

    [Fact]
    public async Task DeleteTeacher_ShouldReturn404_WhenNoObjectExists()
    {
        var response = await _client.DeleteAsync("/api/Teachers/10");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var result = JsonSerializer
            .Deserialize<BadResponseObject>(await response.Content.ReadAsStringAsync(), _serializerOptions);
        result.Should().NotBeNull();
        result?.Message.Should().Be(new ObjectNotFoundByIdException(typeof(Teacher), 10).Message);
    }

    private static async Task CreateTestsData()
    {
        await using var context = ApplicationFactory.Context;
        context.Add(new Faculty { Id = 1, Name = "F1" });
        context.Add(new Department { FacultyId = 1, Id = 1, Name = "D1" });
        context.AddRange(
            new Teacher
            {
                Id = 1,
                LastName = "ln1",
                FirstName = "fn1",
                DepartmentId = 1
            },
            new Teacher
            {
                Id = 2,
                LastName = "ln2",
                FirstName = "fn2",
                DepartmentId = 1
            },
            new Teacher
            {
                Id = 3,
                LastName = "ln3",
                FirstName = "fn3",
                DepartmentId = 1
            },
            new Subject
            {
                Id = 1,
                Name = "subject1"
            },
            new Subject
            {
                Id = 2,
                Name = "subject2"
            },
            new Subject
            {
                Id = 3,
                Name = "subject3"
            });
        context.AddRange(new List<SubjectTeacher>
        {
            new() { TeacherId = 1, SubjectId = 1 },
            new() { TeacherId = 1, SubjectId = 2 },
            new() { TeacherId = 2, SubjectId = 2 },
            new() { TeacherId = 3, SubjectId = 3 }
        });
        await context.SaveChangesAsync();
    }
}