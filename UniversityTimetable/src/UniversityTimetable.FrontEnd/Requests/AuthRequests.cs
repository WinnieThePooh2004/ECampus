using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Requests;

public class AuthRequests : IAuthRequests
{
    private readonly IHttpClientFactory _client;
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    public AuthRequests(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<UserDTO> LoginAsync(LoginDTO login)
    {
        var response = await _client.CreateClient("UTApi").PostAsJsonAsync("api/Auth/login", login);
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<UserDTO>(await response.Content.ReadAsStringAsync(), _options);
    }

    public async Task LogoutAsync()
    {
        var response = await _client.CreateClient("UTApi").DeleteAsync("api/Auth/logout");
    }
}