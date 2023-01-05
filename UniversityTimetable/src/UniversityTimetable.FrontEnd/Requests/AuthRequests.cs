using System.Diagnostics;
using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.Auth;

namespace UniversityTimetable.FrontEnd.Requests;

public class AuthRequests : IAuthRequests
{
    private readonly IHttpClientFactory _client;
    private readonly JsonSerializerOptions _options;
    public AuthRequests(IHttpClientFactory client, JsonSerializerOptions options)
    {
        _client = client;
        _options = options;
    }

    public async Task<LoginResult> LoginAsync(LoginDto login)
    {
        var response = await _client.CreateClient("UTApi").PostAsJsonAsync("api/Auth/login", login);
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<LoginResult>(await response.Content.ReadAsStringAsync(), _options)
            ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }

    public async Task LogoutAsync()
    {
        await _client.CreateClient("UTApi").DeleteAsync("api/Auth/logout");
    }
}