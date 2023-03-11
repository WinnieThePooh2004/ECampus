using System.Net;
using System.Text.Json;
using ECampus.FrontEnd.Requests;
using ECampus.Shared.DataTransferObjects;
using ECampus.Tests.Shared.Mocks.HttpRequests;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Requests;

public class UserRolesRequestsTests
{
    private readonly UserRolesRequests _sut;
    private readonly HttpClientFactory _clientFactory = new();
    
    public UserRolesRequestsTests()
    {
        _sut = new UserRolesRequests(_clientFactory);
    }

    [Fact]
    public async Task GetById_ShouldReturnData_WhenStatusCodeIsSuccessful()
    {
        var auditory = new UserDto();
        var response = new HttpResponseMessage();
        response.Content = new StringContent(JsonSerializer.Serialize(auditory));
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Get, "https://google.com/api/UserRoles/10"), response);

        var result = await _sut.GetByIdAsync(10);

        result.Should().BeEquivalentTo(auditory, opt => opt.ComparingByMembers<AuditoryDto>());
    }
    
    [Fact]
    public async Task GetById_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Get, "https://google.com/api/UserRoles/10"), response);

        await new Func<Task>(() => _sut.GetByIdAsync(10)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }

    [Fact]
    public async Task Create_ShouldReturnData_WhenStatusCodeIsSuccessful()
    {
        var user = new UserDto();
        var response = new HttpResponseMessage();
        response.Content = new StringContent(JsonSerializer.Serialize(user));
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Post, "https://google.com/api/UserRoles"), response);

        var result = await _sut.CreateAsync(user);

        result.Should().BeEquivalentTo(user, opt => opt.ComparingByMembers<AuditoryDto>());
    }
    
    [Fact]
    public async Task Create_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Post, "https://google.com/api/UserRoles"), response);

        await new Func<Task>(() => _sut.CreateAsync(new UserDto())).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
    
    [Fact]
    public async Task Update_ShouldReturnData_WhenStatusCodeIsSuccessful()
    {
        var user = new UserDto();
        var response = new HttpResponseMessage();
        response.Content = new StringContent(JsonSerializer.Serialize(user));
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, "https://google.com/api/UserRoles"), response);

        var result = await _sut.UpdateAsync(user);

        result.Should().BeEquivalentTo(user, opt => opt.ComparingByMembers<AuditoryDto>());
    }
    
    [Fact]
    public async Task Update_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, "https://google.com/api/UserRoles"), response);

        await new Func<Task>(() => _sut.UpdateAsync(new UserDto())).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
}