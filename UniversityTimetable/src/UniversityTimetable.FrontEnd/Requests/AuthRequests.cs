using System.Diagnostics;
using Newtonsoft.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.Auth;

namespace UniversityTimetable.FrontEnd.Requests;

public class AuthRequests : IAuthRequests
{
    private readonly IHttpClientFactory _client;

    public AuthRequests(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<LoginResult> LoginAsync(LoginDto login)
    {
        var response = await _client.CreateClient("UTApi").PostAsJsonAsync("api/Auth/login", login);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<LoginResult>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }

    public async Task LogoutAsync()
    {
        await _client.CreateClient("UTApi").DeleteAsync("api/Auth/logout");
    }
}