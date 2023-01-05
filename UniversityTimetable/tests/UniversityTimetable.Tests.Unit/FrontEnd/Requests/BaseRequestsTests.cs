using System.Net;
using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests;
using UniversityTimetable.FrontEnd.Requests.Options;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Tests.Shared.Mocks.HttpRequests;

namespace UniversityTimetable.Tests.Unit.FrontEnd.Requests;

public class BaseRequestsTests : IDisposable
{
    private readonly BaseRequests<AuditoryDto> _sut;
    private readonly Fixture _fixture = new();

    public BaseRequestsTests()
    {
        var requestsOptions = Substitute.For<IRequestOptions>();
        requestsOptions.GetControllerName(Arg.Any<Type>()).Returns("Auditories");
        _sut = new BaseRequests<AuditoryDto>(new HttpClientFactory(), HttpClientFactory.Options, requestsOptions);
    }
    
    public void Dispose()
    {
        HttpClientFactory.MessageHandler.Responses.Clear();
    }

    [Fact]
    public async Task GetById_ShouldReturnData_WhenStatusCodeIsSuccessful()
    {
        var auditory = _fixture.Create<AuditoryDto>();
        var response = new HttpResponseMessage();
        response.Content = new StringContent(JsonSerializer.Serialize(auditory));
        HttpClientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Get, "https://google.com/api/Auditories/10"), response);

        var result = await _sut.GetByIdAsync(10);

        result.Should().BeEquivalentTo(auditory, opt => opt.ComparingByMembers<AuditoryDto>());
    }
    
    [Fact]
    public async Task GetById_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        HttpClientFactory.MessageHandler.Responses.Add(
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
        HttpClientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Post, "https://google.com/api/Auditories"), response);

        var result = await _sut.CreateAsync(auditory);

        result.Should().BeEquivalentTo(auditory, opt => opt.ComparingByMembers<AuditoryDto>());
    }
    
    [Fact]
    public async Task Create_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        HttpClientFactory.MessageHandler.Responses.Add(
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
        HttpClientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, "https://google.com/api/Auditories"), response);

        var result = await _sut.UpdateAsync(auditory);

        result.Should().BeEquivalentTo(auditory, opt => opt.ComparingByMembers<AuditoryDto>());
    }
    
    [Fact]
    public async Task Update_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        HttpClientFactory.MessageHandler.Responses.Add(
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
        HttpClientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Delete, "https://google.com/api/Auditories/10"), response);

        await _sut.DeleteAsync(10);
    }
    
    [Fact]
    public async Task Delete_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        HttpClientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Delete, "https://google.com/api/Auditories/10"), response);

        await new Func<Task>(() => _sut.DeleteAsync(10)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
}