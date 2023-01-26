using System.Diagnostics;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.FrontEnd.Requests.Options;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using Newtonsoft.Json;

namespace ECampus.FrontEnd.Requests;

public class AuthRequests : IAuthRequests
{
    private readonly IHttpClientFactory _client;

    public AuthRequests(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<LoginResult> LoginAsync(LoginDto login)
    {
        var response = await _client.CreateClient(RequestOptions.ClientName).PostAsJsonAsync("api/Auth/login", login);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<LoginResult>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }

    public async Task LogoutAsync()
    {
        await _client.CreateClient(RequestOptions.ClientName).DeleteAsync("api/Auth/logout");
    }
}