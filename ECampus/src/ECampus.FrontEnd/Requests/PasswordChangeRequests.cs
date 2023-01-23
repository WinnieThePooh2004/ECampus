using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Metadata;

namespace ECampus.FrontEnd.Requests;

[Inject(typeof(IPasswordChangeRequests))]
public class PasswordChangeRequests : IPasswordChangeRequests
{
    private readonly IHttpClientFactory _client;

    public PasswordChangeRequests(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task ChangePassword(PasswordChangeDto passwordChange)
    {
        var response = await _client.CreateClient("UTApi").PutAsJsonAsync("api/Users/changePassword", passwordChange);
        response.EnsureSuccessStatusCode();
    }
}