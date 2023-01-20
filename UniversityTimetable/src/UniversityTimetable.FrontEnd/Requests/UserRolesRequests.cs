using Newtonsoft.Json;

namespace UniversityTimetable.FrontEnd.Requests;

public class UserRolesRequests : IUserRolesRequests
{
    private readonly IHttpClientFactory _client;

    public UserRolesRequests(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<UserDto> CreateAsync(UserDto user)
    {
        using var response = await _client.CreateClient("UTApi").PostAsJsonAsync("api/UserRoles", user);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync())!;
    }
    
    public async Task<UserDto> GetByIdAsync(int id)
    {
        using var response = await _client.CreateClient("UTApi").GetAsync($"api/UserRoles/{id}");
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync())!;
    }
    
    public async Task<UserDto> UpdateAsync(UserDto user)
    {
        using var response = await _client.CreateClient("UTApi").PutAsJsonAsync("api/UserRoles", user);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync())!;
    }
}