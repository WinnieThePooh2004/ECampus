using IdentityServer4.Extensions;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.Extensions;

namespace UniversityTimetable.FrontEnd.Requests;

public class UserRequests : IUserRequests
{
    private readonly IBaseRequests<UserDto> _baseRequests;
    private readonly IHttpClientFactory _client;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserRequests(IBaseRequests<UserDto> baseRequests, IHttpClientFactory client, IHttpContextAccessor httpContextAccessor)
    {
        _baseRequests = baseRequests;
        _client = client;
        _httpContextAccessor = httpContextAccessor;
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
        if (!user.IsAuthenticated())
        {
            throw new UnauthorizedAccessException();
        }
        
        return _baseRequests.GetByIdAsync(user?.GetId() ?? throw new UnauthorizedAccessException());
    }

    public async Task ChangePassword(PasswordChangeDto passwordChange)
    {
        var response = await _client.CreateClient("UTApi").PutAsJsonAsync("api/Users/changePassword", passwordChange);
        response.EnsureSuccessStatusCode();
    }

    public async Task SaveAuditory(int auditoryId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/Users/auditory?userId={UserId()}&auditoryId={auditoryId}");
        var response = await _client.CreateClient("UTApi").SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveSavedAuditory(int auditoryId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/Users/auditory?userId={UserId()}&auditoryId={auditoryId}");
        var response = await _client.CreateClient("UTApi").SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task SaveGroup(int groupId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/Users/group?userId={UserId()}&groupId={groupId}");
        var response = await _client.CreateClient("UTApi").SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveSavedGroup(int groupId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/Users/group?userId={UserId()}&groupId={groupId}");
        var response = await _client.CreateClient("UTApi").SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task SaveTeacher(int teacherId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/Users/teacher?userId={UserId()}&teacherId={teacherId}");
        var response = await _client.CreateClient("UTApi").SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveSavedTeacher(int teacherId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/Users/teacher?userId={UserId()}&teacherId={teacherId}");
        var response = await _client.CreateClient("UTApi").SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    private int UserId()
    {
        return _httpContextAccessor.HttpContext?.User.GetId() ?? throw new UnauthorizedAccessException();
    }
}