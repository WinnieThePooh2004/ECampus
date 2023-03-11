using System.Net;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Validation;
using ECampus.FrontEnd.Requests.ValidationRequests;
using ECampus.Tests.Shared.Mocks.HttpRequests;
using Newtonsoft.Json;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Requests.Validation;

public class ClassValidationRequestsTests
{
    private readonly ClassValidationRequests _sut;
    private readonly HttpClientFactory _clientFactory;
    private readonly Fixture _fixture = new();

    public ClassValidationRequestsTests()
    {
        _clientFactory = new HttpClientFactory();
        _sut = new ClassValidationRequests(_clientFactory);
    }

    [Fact]
    public async Task Validate_ShouldReturnFromClient_WhenStatusCodeValid()
    {
        var validationResult = new ValidationResult(_fixture.CreateMany<ValidationError>(5).ToList());
        var response = new HttpResponseMessage();
        response.Content = new StringContent(JsonConvert.SerializeObject(validationResult));
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, "https://google.com/api/Timetable/Validate"), response);

        var result = await _sut.ValidateAsync(new ClassDto());

        result.Should().BeEquivalentTo(validationResult, opt => opt.ComparingByMembers<ValidationResult>());
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenStatusCodeInvalid()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, "https://google.com/api/Timetable/Validate"), response);

        await new Func<Task>(() => _sut.ValidateAsync(new ClassDto())).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
}