using System.Net;
using System.Text.Json;
using ECampus.FrontEnd.Requests;
using ECampus.FrontEnd.Requests.Options;
using ECampus.Shared.DataTransferObjects;
using ECampus.Tests.Shared.Mocks.HttpRequests;

namespace ECampus.Tests.Unit.FrontEnd.Requests;

public class BaseRequestsTests
{
    private readonly BaseRequests<AuditoryDto> _sut;
    private readonly Fixture _fixture = new();
    private readonly HttpClientFactory _clientFactory = new();

    public BaseRequestsTests()
    {
        var requestsOptions = Substitute.For<IRequestOptions>();
        requestsOptions.GetControllerName(Arg.Any<Type>()).Returns("Auditories");
        _sut = new BaseRequests<AuditoryDto>(_clientFactory, requestsOptions);
    }

    [Fact]
    public async Task GetById_ShouldReturnData_WhenStatusCodeIsSuccessful()
    {
        var auditory = _fixture.Create<AuditoryDto>();
        var response = new HttpResponseMessage();
        response.Content = new StringContent(JsonSerializer.Serialize(auditory));
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Get, "https://google.com/api/Auditories/10"), response);

        var result = await _sut.GetByIdAsync(10);

        result.Should().BeEquivalentTo(auditory, opt => opt.ComparingByMembers<AuditoryDto>());
    }
    
    [Fact]
    public async Task GetById_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Get, "https://google.com/api/Auditories/10"), response);

        await new Func<Task>(() => _sut.GetByIdAsync(10)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }

    [Fact]
    public async Task Create_ShouldReturnData_WhenStatusCodeIsSuccessful()
    {
        var auditory = _fixture.Create<AuditoryDto>();
        var response = new HttpResponseMessage();
        response.Content = new StringContent(JsonSerializer.Serialize(auditory));
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Post, "https://google.com/api/Auditories"), response);

        var result = await _sut.CreateAsync(auditory);

        result.Should().BeEquivalentTo(auditory, opt => opt.ComparingByMembers<AuditoryDto>());
    }
    
    [Fact]
    public async Task Create_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Post, "https://google.com/api/Auditories"), response);

        await new Func<Task>(() => _sut.CreateAsync(new AuditoryDto())).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
    
    [Fact]
    public async Task Update_ShouldReturnData_WhenStatusCodeIsSuccessful()
    {
        var auditory = _fixture.Create<AuditoryDto>();
        var response = new HttpResponseMessage();
        response.Content = new StringContent(JsonSerializer.Serialize(auditory));
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, "https://google.com/api/Auditories"), response);

        var result = await _sut.UpdateAsync(auditory);

        result.Should().BeEquivalentTo(auditory, opt => opt.ComparingByMembers<AuditoryDto>());
    }
    
    [Fact]
    public async Task Update_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, "https://google.com/api/Auditories"), response);

        await new Func<Task>(() => _sut.UpdateAsync(new AuditoryDto())).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
    
    [Fact]
    public async Task Delete_ShouldReturnData_WhenStatusCodeIsSuccessful()
    {
        var auditory = _fixture.Create<AuditoryDto>();
        var response = new HttpResponseMessage();
        response.Content = new StringContent(JsonSerializer.Serialize(auditory));
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Delete, "https://google.com/api/Auditories/10"), response);

        await _sut.DeleteAsync(10);
    }
    
    [Fact]
    public async Task Delete_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Delete, "https://google.com/api/Auditories/10"), response);

        await new Func<Task>(() => _sut.DeleteAsync(10)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
}