using System.Net.Http.Json;
using ECampus.Contracts.DataSelectParameters;
using ECampus.DataAccess.DataSelectors.MultipleItemSelectors;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Models;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Integration.Tests.Endpoints.UserRolesEndpoints;

public class SuccessfulEndpointsTests : IClassFixture<ApplicationFactory>, IAsyncLifetime
{
    private static bool _databaseCreated;
    private readonly HttpClient _client;

    public SuccessfulEndpointsTests(ApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _client.Login(UserRole.Admin);
    }

    public async Task InitializeAsync()
    {
        if (_databaseCreated)
        {
            return;
        }

        _databaseCreated = true;
        await SeedData();
    }

    [Fact]
    public async Task Delete_ShouldSetTeacherTeachersUserEmailAsNull_WhenDeletedUserIsTeacher()
    {
        var response = await _client.DeleteAsync("/api/users/702");

        response.EnsureSuccessStatusCode();
        await using var context = ApplicationFactory.Context;
        var deletedUsersTeacher = await context.Teachers.FindAsync(700);
        deletedUsersTeacher!.UserEmail.Should().Be(null);
    }

    [Fact]
    public async Task Delete_ShouldSetStudentsUserEmailAsNull_WhenDeletedUserIsTeacher()
    {
        var response = await _client.DeleteAsync("/api/users/703");

        response.EnsureSuccessStatusCode();
        await using var context = ApplicationFactory.Context;
        var deletedUsersStudent = await context.Students.FindAsync(700);
        deletedUsersStudent!.UserEmail.Should().Be(null);
    }

    [Fact]
    public async Task CreateAsStudent_ShouldAddEmailToStudent_WhenCreated()
    {
        var newUser = new UserDto
        {
            Id = 704, Email = "user704@email.com", Username = "user704", Role = UserRole.Student,
            Password = "Password1", StudentId = 701
        };

        var response = await _client.PostAsJsonAsync("api/Users", newUser);

        response.EnsureSuccessStatusCode();
        await using var context = ApplicationFactory.Context;
        var createdUsersStudent = await context.Students.FindAsync(701);
        createdUsersStudent!.UserEmail.Should().Be(newUser.Email);
    }

    [Fact]
    public async Task CreateAsTeacher_ShouldAddEmailToTeacher_WhenCreated()
    {
        var newUser = new UserDto
        {
            Id = 705, Email = "user705@email.com", Username = "user705", Role = UserRole.Teacher,
            Password = "Password1", TeacherId = 701
        };

        var response = await _client.PostAsJsonAsync("api/Users", newUser);

        response.EnsureSuccessStatusCode();
        await using var context = ApplicationFactory.Context;
        var createdUsersTeacher = await context.Teachers.FindAsync(701);
        createdUsersTeacher!.UserEmail.Should().Be(newUser.Email);
    }

    [Fact]
    public async Task UpdateAsTeacher_ShouldAddEmailToCurrentAndRemoveFromOld_WhenNewRoleIsTeacher()
    {
        var updatedUser = new UserDto
        {
            Id = 701, Email = "user701@email.com", Username = "user701", Role = UserRole.Teacher,
            Password = "Password1", TeacherId = 703
        };

        var response = await _client.PutAsJsonAsync("api/Users", updatedUser);

        response.EnsureSuccessStatusCode();
        await using var context = ApplicationFactory.Context;
        var userAfterUpdate =
            await new UserRolesSelect().SelectData(context, new UserRolesParameters(701)).SingleAsync();
        userAfterUpdate.StudentId.Should().BeNull();
        userAfterUpdate.TeacherId.Should().Be(703);
        userAfterUpdate.Teacher!.UserEmail.Should().Be(updatedUser.Email);
        var student = await context.FindAsync<Student>(702);
        student!.UserEmail.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsStudent_ShouldAddEmailToCurrentAndRemoveFromOld_WhenNewRoleIsTeacher()
    {
        var updatedUser = new UserDto
        {
            Id = 700, Email = "user700@email.com", Username = "user700", Role = UserRole.Student,
            Password = "Password1", StudentId = 703
        };

        var response = await _client.PutAsJsonAsync("api/Users", updatedUser);

        response.EnsureSuccessStatusCode();
        await using var context = ApplicationFactory.Context;
        var userAfterUpdate =
            await new UserRolesSelect().SelectData(context, new UserRolesParameters(700)).SingleAsync();
        userAfterUpdate.TeacherId.Should().BeNull();
        userAfterUpdate.StudentId.Should().Be(703);
        userAfterUpdate.Student!.UserEmail.Should().Be(updatedUser.Email);
        var teacher = await context.FindAsync<Teacher>(702);
        teacher!.UserEmail.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsStudent_ShouldUpdateOldAndNew_WhenRoleNotChanged()
    {
        var updatedUser = new UserDto
        {
            Id = 707, Email = "user707@email.com", Username = "user707", Role = UserRole.Teacher,
            Password = "Password1", TeacherId = 705
        };

        var response = await _client.PutAsJsonAsync("api/Users", updatedUser);

        response.EnsureSuccessStatusCode();
        await using var context = ApplicationFactory.Context;
        var userAfterUpdate =
            await new UserRolesSelect().SelectData(context, new UserRolesParameters(707)).SingleAsync();
        userAfterUpdate.TeacherId.Should().Be(705);
        userAfterUpdate.Teacher!.UserEmail.Should().Be(updatedUser.Email);
        var oldTeacher = await context.FindAsync<Teacher>(704);
        oldTeacher!.UserEmail.Should().BeNull();
    }
    
    [Fact]
    public async Task UpdateAsTeacher_ShouldUpdateOldAndNew_WhenRoleNotChanged()
    {
        var updatedUser = new UserDto
        {
            Id = 706, Email = "user706@email.com", Username = "user706", Role = UserRole.Student,
            Password = "Password1", StudentId = 705
        };

        var response = await _client.PutAsJsonAsync("api/Users", updatedUser);

        response.EnsureSuccessStatusCode();
        await using var context = ApplicationFactory.Context;
        var userAfterUpdate =
            await new UserRolesSelect().SelectData(context, new UserRolesParameters(706)).SingleAsync();
        userAfterUpdate.StudentId.Should().Be(705);
        userAfterUpdate.Student!.UserEmail.Should().Be(updatedUser.Email);
        var oldStudent = await context.FindAsync<Student>(704);
        oldStudent!.UserEmail.Should().BeNull();
    }

    private static async Task SeedData()
    {
        await using var context = ApplicationFactory.Context;
        var users = new List<User>
        {
            new()
            {
                Id = 700, Email = "user700@email.com", Username = "user700", Role = UserRole.Teacher,
                Teacher = new Teacher { Id = 702, DepartmentId = 1 }, Password = "Password1"
            },
            new()
            {
                Id = 701, Email = "user701@email.com", Username = "user701", Role = UserRole.Student,
                Student = new Student { Id = 702, GroupId = 1 }, Password = "Password1"
            },
            new()
            {
                Id = 702, Email = "user702@email.com", Username = "user702", Role = UserRole.Teacher,
                Teacher = new Teacher { Id = 700, DepartmentId = 1 }
            },
            new()
            {
                Id = 703, Email = "user703@email.com", Username = "user703", Role = UserRole.Student,
                Student = new Student { Id = 700, GroupId = 1 }
            },
            new()
            {
                Id = 706, Email = "user706@email.com", Username = "user706", Role = UserRole.Student,
                Student = new Student { Id = 704, GroupId = 1 }, Password = "Password1"
            },
            new()
            {
                Id = 707, Email = "user707@email.com", Username = "user707", Role = UserRole.Teacher,
                Teacher = new Teacher { Id = 704, DepartmentId = 1 }, Password = "Password1"
            }
        };
        var students = new List<Student>
        {
            new() { Id = 701, GroupId = 1 },
            new() { Id = 703, GroupId = 1 },
            new() { Id = 705, GroupId = 1 }
        };
        var teachers = new List<Teacher>
        {
            new() { Id = 701, DepartmentId = 1 },
            new() { Id = 703, DepartmentId = 1 },
            new() { Id = 705, DepartmentId = 1 }
        };

        context.AddRange(users);
        context.AddRange(students);
        context.AddRange(teachers);
        await context.SaveChangesAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}