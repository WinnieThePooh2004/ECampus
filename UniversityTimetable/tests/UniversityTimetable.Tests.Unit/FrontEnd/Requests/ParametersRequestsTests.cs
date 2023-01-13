using System.Net;
using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests;
using UniversityTimetable.FrontEnd.Requests.Options;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Tests.Shared.Mocks.HttpRequests;

namespace UniversityTimetable.Tests.Unit.FrontEnd.Requests;

public class ParametersRequestsTests : IDisposable
{
    private readonly ParametersRequests<AuditoryDto, AuditoryParameters> _sut;
    private readonly Fixture _fixture = new();
    private readonly HttpClientFactory _clientFactory = new();

    public ParametersRequestsTests()
    {
        var requestsOptions = Substitute.For<IRequestOptions>();
        requestsOptions.GetControllerName(Arg.Any<Type>()).Returns("Auditories");
        _sut = new ParametersRequests<AuditoryDto, AuditoryParameters>(_clientFactory,
            HttpClientFactory.Options, requestsOptions);
    }

    public void Dispose()
    {
        _clientFactory.MessageHandler.Responses.Clear();
    }

    [Fact]
    public async Task GetByParameters_ShouldReturnList_WhenStatusCodeIsSuccessful()
    {
        var list = new ListWithPaginationData<AuditoryDto>
        {
            Metadata = _fixture.Create<PaginationData>(),
            Data = _fixture.CreateMany<AuditoryDto>(10).ToList()
        };
        var parameters = _fixture.Create<AuditoryParameters>();
        var response = new HttpResponseMessage();
        response.Content = new StringContent(JsonSerializer.Serialize(list));
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Get, $"https://google.com/api/Auditories?{parameters.ToQueryString()}"),
            response);

        var result = await _sut.GetByParametersAsync(parameters);

        result.Should().BeEquivalentTo(list,
            opt => opt.ComparingByMembers<ListWithPaginationData<AuditoryDto>>());
    }

    [Fact]
    public async Task GetById_ShouldReturnData_WhenStatusCodeIsSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        var parameters = _fixture.Create<AuditoryParameters>();
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Get, $"https://google.com/api/Auditories?{parameters.ToQueryString()}"),
            response);

        await new Func<Task>(() => _sut.GetByParametersAsync(parameters)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
}