using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.FrontEnd.Requests.Options;
using ECampus.Shared.DataTransferObjects;
using Newtonsoft.Json;

namespace ECampus.FrontEnd.Requests;

public class UserRolesRequests : IUserRolesRequests
{
    private readonly IHttpClientFactory _client;

    public UserRolesRequests(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<UserDto> CreateAsync(UserDto user)
    {
        using var response = await _client.CreateClient(RequestOptions.ClientName).PostAsJsonAsync("api/UserRoles", user);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync())!;
    }
    
    public async Task<UserDto> GetByIdAsync(int id)
    {
        using var response = await _client.CreateClient(RequestOptions.ClientName).GetAsync($"api/UserRoles/{id}");
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync())!;
    }
    
    public async Task<UserDto> UpdateAsync(UserDto user)
    {
        using var response = await _client.CreateClient(RequestOptions.ClientName).PutAsJsonAsync("api/UserRoles", user);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync())!;
    }
}