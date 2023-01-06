using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using UniversityTimetable.FrontEnd.Requests;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.Auth;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Tests.Shared.Extensions;
using UniversityTimetable.Tests.Shared.Mocks.HttpRequests;

namespace UniversityTimetable.Tests.Unit.FrontEnd.Requests;

public class UserRequestsTests : IDisposable
{
    private readonly UserRequests _sut;
    private readonly IBaseRequests<UserDto> _baseRequests = Substitute.For<IBaseRequests<UserDto>>();
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly Fixture _fixture = new();
    private readonly HttpContext _httpContext = Substitute.For<HttpContext>();

    private readonly ClaimsPrincipal _user = new(new ClaimsIdentity(new List<Claim>
    {
        new(CustomClaimTypes.Id, "10")
    }));

    public UserRequestsTests()
    {
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _sut = new UserRequests(_baseRequests, new HttpClientFactory(), _httpContextAccessor,
            HttpClientFactory.Options);
        _httpContextAccessor.HttpContext.Returns(_httpContext);
        _httpContext.User.Returns(_user);
    }

    public void Dispose()
    {
        HttpClientFactory.MessageHandler.Responses.Clear();
    }

    [Fact]
    public async Task GetById_ShouldCallBaseRequests()
    {
        var user = _fixture.Create<UserDto>();
        _baseRequests.GetByIdAsync(10).Returns(user);

        var result = await _sut.GetByIdAsync(10);

        result.Should().Be(user);
        await _baseRequests.Received(1).GetByIdAsync(10);
    }

    [Fact]
    public async Task Create_ShouldCallBaseRequests()
    {
        var user = _fixture.Create<UserDto>();
        _baseRequests.CreateAsync(user).Returns(user);

        var result = await _sut.CreateAsync(user);

        result.Should().Be(user);
        await _baseRequests.Received(1).CreateAsync(user);
    }

    [Fact]
    public async Task Delete_ShouldCallBaseRequests()
    {
        await _sut.DeleteAsync(10);

        await _baseRequests.Received(1).DeleteAsync(10);
    }

    [Fact]
    public async Task Update_ShouldCallBaseRequests()
    {
        var user = _fixture.Create<UserDto>();
        _baseRequests.UpdateAsync(user).Returns(user);

        var result = await _sut.UpdateAsync(user);

        result.Should().Be(user);
        await _baseRequests.Received(1).UpdateAsync(user);
    }

    [Fact]
    public async Task SaveAuditory_ShouldCallHttpClient_WhenResultIsNoContent()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent };
        HttpClientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Post, "https://google.com/api/Users/auditory?userId=10&auditoryId=10"),
            response);

        await new Func<Task>(() => _sut.SaveAuditory(10)).Should().NotThrowAsync();
    }

    [Fact]
    public async Task SaveAuditory_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        HttpClientFactory.MessageHandler.Responses.Add(
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
        HttpClientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Post, "https://google.com/api/Users/group?userId=10&groupId=10"),
            response);

        await new Func<Task>(() => _sut.SaveGroup(10)).Should().NotThrowAsync();
    }

    [Fact]
    public async Task SaveGroup_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        HttpClientFactory.MessageHandler.Responses.Add(
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
        HttpClientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Post, "https://google.com/api/Users/teacher?userId=10&teacherId=10"),
            response);

        await new Func<Task>(() => _sut.SaveTeacher(10)).Should().NotThrowAsync();
    }

    [Fact]
    public async Task SaveTeacher_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        HttpClientFactory.MessageHandler.Responses.Add(
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
        HttpClientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Delete, "https://google.com/api/Users/auditory?userId=10&auditoryId=10"),
            response);

        await new Func<Task>(() => _sut.RemoveSavedAuditory(10)).Should().NotThrowAsync();
    }

    [Fact]
    public async Task RemoveSavedAuditory_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        HttpClientFactory.MessageHandler.Responses.Add(
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
        HttpClientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Delete, "https://google.com/api/Users/group?userId=10&groupId=10"),
            response);

        await new Func<Task>(() => _sut.RemoveSavedGroup(10)).Should().NotThrowAsync();
    }

    [Fact]
    public async Task RemoveSavedGroup_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        HttpClientFactory.MessageHandler.Responses.Add(
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
        HttpClientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Delete, "https://google.com/api/Users/teacher?userId=10&teacherId=10"),
            response);

        await new Func<Task>(() => _sut.RemoveSavedTeacher(10)).Should().NotThrowAsync();
    }

    [Fact]
    public async Task RemoveSavedTeacher_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        HttpClientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Delete, "https://google.com/api/Users/teacher?userId=10&teacherId=10"),
            response);

        await new Func<Task>(() => _sut.RemoveSavedTeacher(10)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }

    [Fact]
    public async Task GetCurrentUser_ShouldThrowException_IfUserDoesNotHaveIdClaim()
    {
        _httpContext.User.Returns(new ClaimsPrincipal());

        await new Func<Task>(() => _sut.GetCurrentUserAsync()).Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }
    
    [Fact]
    public async Task GetCurrentUser_ShouldThrowException_IfNotAuthorized()
    {
        _httpContextAccessor.HttpContext = null;

        await new Func<Task>(() => _sut.GetCurrentUserAsync()).Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetCurrentUser_ShouldReturnGetByIdOfCurrentUser_IfNoExceptionsThrown()
    {
        var user = new UserDto();
        _baseRequests.GetByIdAsync(10).Returns(user);

        var result = await _sut.GetByIdAsync(10);

        result.Should().Be(user);
        await _baseRequests.Received(1).GetByIdAsync(10);
    }

    [Fact]
    public async Task ValidateCreateAsync_ReturnsFromResponse_IfStatusCodeSuccessful()
    {
        var errors = _fixture.CreateMany<KeyValuePair<string, string>>().ToList();
        var response = new HttpResponseMessage();
        response.Content = new StringContent(JsonSerializer.Serialize(errors));
        HttpClientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, "https://google.com/api/Users/Validate/Create"), response);

        var result = await _sut.ValidateCreateAsync(new UserDto());

        result.Should().ContainsKeysWithValues(errors);
    }

    [Fact]
    public async Task ValidateUpdateAsync_ReturnsFromResponse_IfStatusCodeSuccessful()
    {
        var errors = _fixture.CreateMany<KeyValuePair<string, string>>().ToList();
        var response = new HttpResponseMessage();
        response.Content = new StringContent(JsonSerializer.Serialize(errors));
        HttpClientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, "https://google.com/api/Users/Validate/Update"), response);

        var result = await _sut.ValidateUpdateAsync(new UserDto());

        result.Should().ContainsKeysWithValues(errors);
    }

    [Fact]
    public async Task ValidateCreate_ShouldThrowException_WhenResponseStatusCodeNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        HttpClientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, "https://google.com/api/Users/Validate/Create"), response);

        await new Func<Task>(() => _sut.ValidateCreateAsync(new UserDto())).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
    
    [Fact]
    public async Task ValidateUpdate_ShouldThrowException_WhenResponseStatusCodeNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        HttpClientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, "https://google.com/api/Users/Validate/Update"), response);

        await new Func<Task>(() => _sut.ValidateUpdateAsync(new UserDto())).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
}