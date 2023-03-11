using System.Net;
using System.Text.Json;
using ECampus.FrontEnd.Requests;
using ECampus.Tests.Shared.DataFactories;
using ECampus.Tests.Shared.Mocks.HttpRequests;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Requests;

public class TimetableRequestsTests
{
    private readonly ClassRequests _sut;
    private readonly Fixture _fixture = new();
    private readonly TimetableFactory _timetableFactory = new();
    private readonly HttpClientFactory _clientFactory;

    public TimetableRequestsTests()
    {
        _clientFactory = new HttpClientFactory();
        _sut = new ClassRequests(_clientFactory);
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
    
    [Fact]
    public async Task GetTimetableForAuditory_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Get, "https://google.com/api/Timetable/Auditory/10"), response);

        await new Func<Task>(() => _sut.AuditoryTimetable(10)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
    
    [Fact]
    public async Task GetTimetableForAuditory_ShouldReturnSerializedTimetable_WhenStatusCodeIsOk()
    {
        var timetable = _timetableFactory.CreateModel(_fixture);
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(timetable))
        };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Get, $"https://google.com/api/Timetable/Auditory/10"), response);

        var result = await _sut.AuditoryTimetable(10);

        result.Should().BeEquivalentTo(timetable);
    }
    
    [Fact]
    public async Task GetTimetableForGroup_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Get, "https://google.com/api/Timetable/Group/10"), response);

        await new Func<Task>(() => _sut.GroupTimetable(10)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
    
    [Fact]
    public async Task GetTimetableForGroup_ShouldReturnSerializedTimetable_WhenStatusCodeIsOk()
    {
        var timetable = _timetableFactory.CreateModel(_fixture);
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(timetable))
        };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Get, "https://google.com/api/Timetable/Group/10"), response);

        var result = await _sut.GroupTimetable(10);

        result.Should().BeEquivalentTo(timetable);
    }

    [Fact]
    public async Task GetTimetableForTeacher_ShouldThrowException_WhenStatusCodeIsNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Get, "https://google.com/api/Timetable/Teacher/10"), response);

        await new Func<Task>(() => _sut.TeacherTimetable(10)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
    
    [Fact]
    public async Task GetTimetableForTeacher_ShouldReturnSerializedTimetable_WhenStatusCodeIsOk()
    {
        var timetable = _timetableFactory.CreateModel(_fixture);
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(timetable))
        };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Get, "https://google.com/api/Timetable/Teacher/10"), response);

        var result = await _sut.TeacherTimetable(10);

        result.Should().BeEquivalentTo(timetable);
    }
}