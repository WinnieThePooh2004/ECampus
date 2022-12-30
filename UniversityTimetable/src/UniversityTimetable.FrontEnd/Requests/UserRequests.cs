using System.Diagnostics;
using System.Text.Json;
using IdentityServer4.Extensions;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.Auth;

namespace UniversityTimetable.FrontEnd.Requests;

public class UserRequests : IUserRequests
{
    private readonly IBaseRequests<UserDto> _baseRequests;
    private readonly IHttpClientFactory _client;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JsonSerializerOptions _options;
    
    public UserRequests(IBaseRequests<UserDto> baseRequests, IHttpClientFactory client, IHttpContextAccessor httpContextAccessor, JsonSerializerOptions options)
    {
        _baseRequests = baseRequests;
        _client = client;
        _httpContextAccessor = httpContextAccessor;
        _options = options;
    }

    public Task<UserDto> GetByIdAsync(int id)
        => _baseRequests.GetByIdAsync(id);

    public Task<UserDto> CreateAsync(UserDto entity)
        => _baseRequests.CreateAsync(entity);

    public Task<UserDto> UpdateAsync(UserDto entity)
        => _baseRequests.UpdateAsync(entity);

    public Task DeleteAsync(int id)
        => _baseRequests.DeleteAsync(id);

    public Task<UserDto> GetCurrentUserAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user is null || !user.IsAuthenticated())
        {
            throw new UnauthorizedAccessException();
        }

        var id = user.Claims.FirstOrDefault(claim => claim.Type == CustomClaimTypes.Id)?.Value ?? throw new UnauthorizedAccessException();
        return _baseRequests.GetByIdAsync(int.Parse(id));
    }

    public async Task<List<KeyValuePair<string, string>>> ValidateCreateAsync(UserDto user)
    {
        var response = await _client.CreateClient("UTApi").PutAsJsonAsync("api/Users/Validate/Create", user);
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(await response.Content.ReadAsStringAsync(), _options)
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }

    public async Task<List<KeyValuePair<string, string>>> ValidateUpdateAsync(UserDto user)
    {
        var response = await _client.CreateClient("UTApi").PutAsJsonAsync("api/Users/Validate/Update", user);
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(await response.Content.ReadAsStringAsync(), _options)
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }

    public async Task SaveAuditory(int auditoryId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/Users/auditory/{auditoryId}");
        var response = await _client.CreateClient("UTApi").SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveSavedAuditory(int auditoryId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/Users/auditory/{auditoryId}");
        var response = await _client.CreateClient("UTApi").SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task SaveGroup(int groupId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/Users/group/{groupId}");
        var response = await _client.CreateClient("UTApi").SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveSavedGroup(int groupId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/Users/group/{groupId}");
        var response = await _client.CreateClient("UTApi").SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task SaveTeacher(int teacherId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/Users/teacher/{teacherId}");
        var response = await _client.CreateClient("UTApi").SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveSavedTeacher(int teacherId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/Users/teacher/{teacherId}");
        var response = await _client.CreateClient("UTApi").SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}