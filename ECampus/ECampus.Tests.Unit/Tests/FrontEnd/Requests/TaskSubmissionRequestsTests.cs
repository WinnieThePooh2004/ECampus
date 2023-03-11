using System.Net;
using ECampus.Domain.DataTransferObjects;
using ECampus.FrontEnd.Requests;
using ECampus.FrontEnd.Requests.Options;
using ECampus.Tests.Shared.Mocks.HttpRequests;
using Newtonsoft.Json;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Requests;

public class TaskSubmissionRequestsTests
{
    private readonly TaskSubmissionRequests _sut;
    private readonly string _controllerName;
    private readonly HttpClientFactory _clientFactory = new();
    private readonly Fixture _fixture = new();

    public TaskSubmissionRequestsTests()
    {
        var options = Substitute.For<IRequestOptions>();
        options.GetControllerName(Arg.Any<Type>()).Returns("");
        _controllerName = options.GetControllerName(typeof(TaskSubmissionDto));
        _sut = new TaskSubmissionRequests(_clientFactory, options);
    }
    
    [Fact]
    public async Task UpdateMark_ShouldThrowException_WhenStatusCodeNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, $"https://google.com/api/{_controllerName}/mark"),
            response);
    
        await new Func<Task>(() => _sut.UpdateMarkAsync(10, 10)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
    
    [Fact]
    public async Task UpdateMark_ShouldCallHttpClient_WhenResultIsNoContent()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, $"https://google.com/api/{_controllerName}/mark"),
            response);
    
        await new Func<Task>(() => _sut.UpdateMarkAsync(10, 10)).Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task UpdateContent_ShouldThrowException_WhenStatusCodeNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, $"https://google.com/api/{_controllerName}/content"),
            response);
    
        await new Func<Task>(() => _sut.UpdateContextAsync(10, "abc")).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
    
    [Fact]
    public async Task UpdateContent_ShouldCallHttpClient_WhenResultIsNoContent()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, $"https://google.com/api/{_controllerName}/content"),
            response);
    
        await new Func<Task>(() => _sut.UpdateContextAsync(10, "abc")).Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task GetByCourse_ShouldReturnData_WhenStatusCodeIsSuccessful()
    {
        var submission = _fixture.Create<TaskSubmissionDto>();
        var response = new HttpResponseMessage();
        response.Content = new StringContent(JsonConvert.SerializeObject(submission));
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Get, $"https://google.com/api/{_controllerName}/byCourseTask/10"), response);

        var result = await _sut.GetByCourseTaskAsync(10);

        result.Should().BeEquivalentTo(submission);
    }
    
    [Fact]
    public async Task GetByCourse_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Get, $"https://google.com/api/{_controllerName}/byCourseTask/10"), response);

        await new Func<Task>(() => _sut.GetByCourseTaskAsync(10)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
}