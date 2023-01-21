using System.Net;
using System.Security.Claims;
using ECampus.FrontEnd.Requests;
using ECampus.Shared.Auth;
using ECampus.Tests.Shared.Mocks.HttpRequests;
using Microsoft.AspNetCore.Http;

namespace ECampus.Tests.Unit.FrontEnd.Requests;

public class UserRequestsTests
{
    private readonly UserRelationshipsRequests _sut;
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly Fixture _fixture = new();
    private readonly HttpContext _httpContext = Substitute.For<HttpContext>();
    private readonly HttpClientFactory _clientFactory = new();

    private readonly ClaimsPrincipal _user = new(new ClaimsIdentity(new List<Claim>
    {
        new(CustomClaimTypes.Id, "10")
    }));

    public UserRequestsTests()
    {
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _sut = new UserRelationshipsRequests(_clientFactory, _httpContextAccessor);
        _httpContextAccessor.HttpContext.Returns(_httpContext);
        _httpContext.User.Returns(_user);
    }
    
    [Fact]
    public async Task SaveAuditory_ShouldCallHttpClient_WhenResultIsNoContent()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Post, "https://google.com/api/Users/auditory?userId=10&auditoryId=10"),
            response);

        await new Func<Task>(() => _sut.SaveAuditory(10)).Should().NotThrowAsync();
    }

    [Fact]
    public async Task SaveAuditory_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Post, "https://google.com/api/Users/auditory?userId=10&auditoryId=10"),
            response);

        await new Func<Task>(() => _sut.SaveAuditory(10)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }

    [Fact]
    public async Task SaveGroup_ShouldCallHttpClient_WhenResultIsNoContent()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Post, "https://google.com/api/Users/group?userId=10&groupId=10"),
            response);

        await new Func<Task>(() => _sut.SaveGroup(10)).Should().NotThrowAsync();
    }

    [Fact]
    public async Task SaveGroup_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Post, "https://google.com/api/Users/group?userId=10&groupId=10"),
            response);

        await new Func<Task>(() => _sut.SaveGroup(10)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }

    [Fact]
    public async Task SaveTeacher_ShouldCallHttpClient_WhenResultIsNoContent()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Post, "https://google.com/api/Users/teacher?userId=10&teacherId=10"),
            response);

        await new Func<Task>(() => _sut.SaveTeacher(10)).Should().NotThrowAsync();
    }

    [Fact]
    public async Task SaveTeacher_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Post, "https://google.com/api/Users/teacher?userId=10&teacherId=10"),
            response);

        await new Func<Task>(() => _sut.SaveTeacher(10)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }

    [Fact]
    public async Task RemoveSavedAuditory_ShouldCallHttpClient_WhenResultIsNoContent()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Delete, "https://google.com/api/Users/auditory?userId=10&auditoryId=10"),
            response);

        await new Func<Task>(() => _sut.RemoveSavedAuditory(10)).Should().NotThrowAsync();
    }

    [Fact]
    public async Task RemoveSavedAuditory_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Delete, "https://google.com/api/Users/auditory?userId=10&auditoryId=10"),
            response);

        await new Func<Task>(() => _sut.RemoveSavedAuditory(10)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }

    [Fact]
    public async Task RemoveSavedGroup_ShouldCallHttpClient_WhenResultIsNoContent()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Delete, "https://google.com/api/Users/group?userId=10&groupId=10"),
            response);

        await new Func<Task>(() => _sut.RemoveSavedGroup(10)).Should().NotThrowAsync();
    }

    [Fact]
    public async Task RemoveSavedGroup_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Delete, "https://google.com/api/Users/group?userId=10&groupId=10"),
            response);

        await new Func<Task>(() => _sut.RemoveSavedGroup(10)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }

    [Fact]
    public async Task RemoveSavedTeacher_ShouldCallHttpClient_WhenResultIsNoContent()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Delete, "https://google.com/api/Users/teacher?userId=10&teacherId=10"),
            response);

        await new Func<Task>(() => _sut.RemoveSavedTeacher(10)).Should().NotThrowAsync();
    }

    [Fact]
    public async Task RemoveSavedTeacher_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Delete, "https://google.com/api/Users/teacher?userId=10&teacherId=10"),
            response);

        await new Func<Task>(() => _sut.RemoveSavedTeacher(10)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }

    // [Fact]
    // public async Task ChangePassword_ShouldThrowException_WhenStatusCodeNotSuccessful()
    // {
    //     var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
    //     _clientFactory.MessageHandler.Responses.Add(
    //         new HttpRequestMessage(HttpMethod.Put, "https://google.com/api/Users/changePassword"),
    //         response);
    //
    //     await new Func<Task>(() => _sut.ChangePassword(new PasswordChangeDto())).Should()
    //         .ThrowAsync<HttpRequestException>()
    //         .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    // }
    //
    // [Fact]
    // public async Task ChangePassword_ShouldCallHttpClient_WhenResultIsNoContent()
    // {
    //     var response = new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent };
    //     _clientFactory.MessageHandler.Responses.Add(
    //         new HttpRequestMessage(HttpMethod.Put, "https://google.com/api/Users/changePassword"),
    //         response);
    //
    //     await new Func<Task>(() => _sut.ChangePassword(new PasswordChangeDto())).Should().NotThrowAsync();
    // }
}