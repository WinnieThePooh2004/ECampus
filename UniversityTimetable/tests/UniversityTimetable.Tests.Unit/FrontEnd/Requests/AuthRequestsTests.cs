using System.Net;
using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Tests.Shared.Mocks.HttpRequests;

namespace UniversityTimetable.Tests.Unit.FrontEnd.Requests;

public class AuthRequestsTests
{
    private readonly AuthRequests _sut;
    private readonly HttpClientFactory _clientFactory = new();
    private readonly Fixture _fixture = new();

    public AuthRequestsTests()
    {
        _sut = new AuthRequests(_clientFactory);
    }

    [Fact]
    public async Task Login_ShouldReturnData_WhenStatusCodeIsSuccessful()
    {
        var login = _fixture.Create<LoginDto>();
        var response = new HttpResponseMessage();
        response.Content = new StringContent(JsonSerializer.Serialize(login));
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Post, "https://google.com/api/Auth/login"), response);

        await _sut.LoginAsync(login);
    }
    
    [Fact]
    public async Task Login_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Post, "https://google.com/api/Auth/login"), response);

        await new Func<Task>(() => _sut.LoginAsync(new LoginDto())).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
    
    [Fact]
    public async Task Logout_ShouldLogout()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Delete, "https://google.com/api/Auth/logout"), response);

        await _sut.LogoutAsync();
    }
}