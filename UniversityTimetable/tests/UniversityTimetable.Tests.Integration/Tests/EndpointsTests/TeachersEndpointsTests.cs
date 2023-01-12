﻿using System.Net;
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

public class TeachersEndpointsTests : IClassFixture<ApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions = HttpClientFactory.Options;

    public TeachersEndpointsTests(ApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _client.Login(DefaultUsers.Admin);
    }

    public async Task InitializeAsync()
    {
        await CreateTestsData();
    }
    
    public async Task DisposeAsync()
    {
        await ApplicationFactory.Context.Database.EnsureDeletedAsync();
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
        teacher?.Subjects?.Count.Should().Be(2);
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
        var response = await _client.DeleteAsync("/api/Teachers/1");
        response.EnsureSuccessStatusCode();
        (await ApplicationFactory.Context.Teachers.CountAsync()).Should().Be(2);
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
        var context = ApplicationFactory.Context;
        context.AddRange(
            new Teacher
            {
                Id = 1,
                LastName = "ln1",
                FirstName = "fn1"
            },
            new Teacher
            {
                Id = 2,
                LastName = "ln2",
                FirstName = "fn2"
            },
            new Teacher
            {
                Id = 3,
                LastName = "ln3",
                FirstName = "fn3"
            },
            new Subject
            {
                Id = 1,
                Name = "subject1",
                TeacherIds = new List<SubjectTeacher>
                {
                    new() { TeacherId = 1 }
                }
            },
            new Subject
            {
                Id = 2,
                Name = "subject2",
                TeacherIds = new List<SubjectTeacher>
                {
                    new() { TeacherId = 1 },
                    new() { TeacherId = 2 }
                }
            },
            new Subject
            {
                Id = 3,
                Name = "subject3",
                TeacherIds = new List<SubjectTeacher>
                {
                    new() { TeacherId = 3 }
                }
            });
        await context.SaveChangesAsync();
    }
}