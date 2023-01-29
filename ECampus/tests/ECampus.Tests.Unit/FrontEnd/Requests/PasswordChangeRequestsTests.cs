using System.Net;
using ECampus.FrontEnd.Requests;
using ECampus.Shared.DataTransferObjects;
using ECampus.Tests.Shared.Mocks.HttpRequests;

namespace ECampus.Tests.Unit.FrontEnd.Requests;

public class PasswordChangeRequestsTests
{
    private readonly PasswordChangeRequests _sut;
    private readonly HttpClientFactory _clientFactory;

    public PasswordChangeRequestsTests()
    {
        _clientFactory = new HttpClientFactory();
        _sut = new PasswordChangeRequests(_clientFactory);
    }
    
    [Fact]
    public async Task ChangePassword_ShouldThrowException_WhenStatusCodeNotSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, "https://google.com/api/Users/changePassword"),
            response);
    
        await new Func<Task>(() => _sut.ChangePassword(new PasswordChangeDto())).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
    
    [Fact]
    public async Task ChangePassword_ShouldCallHttpClient_WhenResultIsNoContent()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent };
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Put, "https://google.com/api/Users/changePassword"),
            response);
    
        await new Func<Task>(() => _sut.ChangePassword(new PasswordChangeDto())).Should().NotThrowAsync();
    }
}