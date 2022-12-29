using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

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

    public async Task<UserDto> LoginAsync(LoginDto login)
    {
        var response = await _client.CreateClient("UTApi").PostAsJsonAsync("api/Auth/login", login);
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<UserDto>(await response.Content.ReadAsStringAsync(), _options);
    }

    public async Task LogoutAsync()
    {
        await _client.CreateClient("UTApi").DeleteAsync("api/Auth/logout");
    }
}