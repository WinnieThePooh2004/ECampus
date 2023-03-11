using System.Net;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Validation;
using ECampus.FrontEnd.Requests.ValidationRequests;
using ECampus.Tests.Shared.Mocks.HttpRequests;
using Newtonsoft.Json;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Requests.Validation;

public class UserCreateValidationRequestsTests
{
    private readonly UserCreateValidationRequests _sut;
    private readonly HttpClientFactory _clientFactory;
    private readonly Fixture _fixture = new();

    public UserCreateValidationRequestsTests()
    {
        _clientFactory = new HttpClientFactory();
        _sut = new UserCreateValidationRequests(_clientFactory);
    }

    [Fact]
    public async Task Validate_ShouldReturnFromClient_WhenStatusCodeValid()
    {
        var validationResult = new ValidationResult(_fixture.CreateMany<ValidationError>(5).ToList());
        var response = new HttpResponseMessage();
        response.Content = new StringContent(JsonConvert.SerializeObject(validationResult));
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, "https://google.com/api/Users/Validate/Create"), response);

        var result = await _sut.ValidateAsync(new UserDto());

        result.Should().BeEquivalentTo(validationResult, opt => opt.ComparingByMembers<ValidationResult>());
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenStatusCodeInvalid()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, "https://google.com/api/Users/Validate/Create"), response);

        await new Func<Task>(() => _sut.ValidateAsync(new UserDto())).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
}