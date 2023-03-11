using System.Net.Http.Json;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Tests.Integration.AppFactories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ECampus.Tests.Integration.Tests.Endpoints;

public class AuthEndpointsTests : IClassFixture<ApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthEndpointsTests(ApplicationFactory app)
    {
        _client = app.CreateClient();
    }

    [Fact]
    public async Task SignUp_ShouldCreateUser_When200IsReturned()
    {
        var registration = new RegistrationDto
        {
            Password = "Password1234", PasswordConfirm = "Password1234", Email = "email800@email.com",
            Username = "UniqueUsername800"
        };

        var response = await _client.PostAsJsonAsync("api/Auth/signup", registration);

        response.EnsureSuccessStatusCode();
        await using var context = ApplicationFactory.Context;
        var users = await context.Users.SingleAsync(user => user.Email == registration.Email);
        users.Username.Should().Be(registration.Username);
    }

    [Fact]
    public async Task Login_ShouldReturnLoginResult_WhenUserExist()
    {
        var user = new User
        {
            Id = 801, Email = "abc.example801.com", Password = "strongPassword1",
            Student = new Student { Id = 800, GroupId = 1 }, StudentId = 800
        };
        await using var context = ApplicationFactory.Context;
        context.Add(user);
        await context.SaveChangesAsync();

        var response = await _client
            .PostAsJsonAsync("api/Auth/login", new LoginDto { Email = user.Email, Password = user.Password });

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var loginResult = JsonConvert.DeserializeObject<LoginResult>(content);
        loginResult!.Email.Should().Be(user.Email);
        loginResult.GroupId.Should().Be(1);
        loginResult.StudentId.Should().Be(800);
    }
}