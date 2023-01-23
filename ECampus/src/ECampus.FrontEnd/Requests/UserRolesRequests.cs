using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Metadata;
using Newtonsoft.Json;

namespace ECampus.FrontEnd.Requests;

[Inject(typeof(IUserRolesRequests))]
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