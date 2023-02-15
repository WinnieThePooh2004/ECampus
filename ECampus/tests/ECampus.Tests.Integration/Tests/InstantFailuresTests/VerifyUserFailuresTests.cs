using System.Net;
using ECampus.Shared.Enums;
using ECampus.Shared.Models;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using FluentAssertions;

namespace ECampus.Tests.Integration.Tests.InstantFailuresTests;

public class VerifyUserFailuresTests : IClassFixture<ApplicationWithoutDatabase>
{
    private readonly HttpClient _client;

    public VerifyUserFailuresTests(ApplicationWithoutDatabase factory)
    {
        _client = factory.CreateClient();
        var user = DefaultUsers.GetUserByRole(UserRole.Guest);
        factory.Context.Users = new DbSetMock<User>(user);
        _client.Login(user);
    }

    [Fact]
    public async Task UserAuditoryPost_ShouldReturn400_WhenUserIsNotAdminAndIdsNotSame()
    {
        var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "api/UserProfile/auditory?userId=1"));
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UserAuditoryDelete_ShouldReturn400_WhenUserIsNotAdminAndIdsNotSame()
    {
        var response =
            await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "api/UserProfile/auditory?userId=1"));
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UserGroupPost_ShouldReturn400_WhenUserIsNotAdminAndIdsNotSame()
    {
        var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "api/UserProfile/group?userId=1"));
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UserGroupDelete_ShouldReturn400_WhenUserIsNotAdminAndIdsNotSame()
    {
        var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "api/UserProfile/group?userId=1"));
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UserTeacherPost_ShouldReturn400_WhenUserIsNotAdminAndIdsNotSame()
    {
        var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "api/UserProfile/teacher?userId=1"));
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UserTeacherDelete_ShouldReturn400_WhenUserIsNotAdminAndIdsNotSame()
    {
        var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "api/UserProfile/teacher?userId=1"));
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}