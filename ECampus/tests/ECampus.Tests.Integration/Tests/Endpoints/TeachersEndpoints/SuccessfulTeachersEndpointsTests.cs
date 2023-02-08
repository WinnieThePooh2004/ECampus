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
        var response = await _client.GetAsync("/api/Teachers/-1");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var result = JsonSerializer
            .Deserialize<BadResponseObject>(await response.Content.ReadAsStringAsync(), _serializerOptions);
        result.Should().NotBeNull();
        result?.Message.Should().Be(new ObjectNotFoundByIdException(typeof(Teacher), -1).Message);
    }

    [Fact]
    public async Task GetTeacher_ShouldReturnTeacherAndSubjects_IfTeacherExists()
    {
        var response = await _client.GetAsync($"/api/Teachers/{301}");
        response.EnsureSuccessStatusCode();
        var teacher =
            JsonSerializer.Deserialize<TeacherDto>(await response.Content.ReadAsStringAsync(), _serializerOptions);
        teacher.Should().NotBeNull();
        teacher!.Subjects!.Should().NotBeNull();
        teacher.Subjects!.Count.Should().Be(1);
        teacher.Id.Should().Be(301);
    }

    [Fact]
    public async Task UpdateTeacher_ShouldUpdateRelationModels_WhenNoValidationErrorOccured()
    {
        var teacher = new TeacherDto
        {
            Id = 300,
            LastName = "ln1",
            FirstName = "fn1",
            DepartmentId = 1,
            Subjects = new List<SubjectDto> { new() { Id = 300 }, new() { Id = 302 } }
        };
        var response = await _client.PutAsJsonAsync("/api/Teachers", teacher);
        response.EnsureSuccessStatusCode();

        var context = ApplicationFactory.Context;
        var subjectTeachers = await context.SubjectTeachers.Where(st => st.TeacherId == 300)
            .Select(st => st.SubjectId)
            .ToListAsync();
        subjectTeachers.Should().Contain(300);
        subjectTeachers.Should().Contain(302);
        subjectTeachers.Should().NotContain(301);
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
            Id = 304,
            LastName = "lastname",
            FirstName = "firstName",
            DepartmentId = 1,
            Subjects = new List<SubjectDto> { new() { Id = 301 }, new() { Id = 302 } }
        };

        var response = await _client.PostAsJsonAsync("/api/Teachers", teacher);

        response.EnsureSuccessStatusCode();
        await using var context = ApplicationFactory.Context;
        (await context.Teachers.FindAsync(304)).Should().NotBeNull();
        (await context.SubjectTeachers.Where(st => st.TeacherId == 304).CountAsync()).Should().Be(2);
    }

    [Fact]
    public async Task CreateTeacher_ShouldReturn400_WhenValidationErrorOccured()
    {
        var teacher = new TeacherDto { Id = 350 };

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
        var response = await _client.DeleteAsync("/api/Teachers/302");
        response.EnsureSuccessStatusCode();

        await using var context = ApplicationFactory.Context;
        (await context.Teachers.SingleOrDefaultAsync(t => t.Id == 302)).Should().BeNull();
    }

    [Fact]
    public async Task DeleteTeacher_ShouldReturn404_WhenNoObjectExists()
    {
        var response = await _client.DeleteAsync("/api/Teachers/-1");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static async Task CreateTestsData()
    {
        await using var context = ApplicationFactory.Context;
        context.AddRange(
            new Teacher
            {
                Id = 300,
                LastName = "ln1",
                FirstName = "fn1",
                DepartmentId = 1
            },
            new Teacher
            {
                Id = 301,
                LastName = "ln2",
                FirstName = "fn2",
                DepartmentId = 1
            },
            new Teacher
            {
                Id = 302,
                LastName = "ln3",
                FirstName = "fn3",
                DepartmentId = 1
            },
            new Subject
            {
                Id = 300,
                Name = "subject1"
            },
            new Subject
            {
                Id = 301,
                Name = "subject2"
            },
            new Subject
            {
                Id = 302,
                Name = "subject3"
            });
        context.AddRange(new List<SubjectTeacher>
        {
            new() { TeacherId = 300, SubjectId = 300 },
            new() { TeacherId = 300, SubjectId = 301 },
            new() { TeacherId = 301, SubjectId = 301 },
            new() { TeacherId = 302, SubjectId = 302 }
        });
        await context.SaveChangesAsync();
    }
}