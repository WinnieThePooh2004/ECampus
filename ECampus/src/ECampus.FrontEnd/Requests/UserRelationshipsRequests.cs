using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.FrontEnd.Requests.Options;
using ECampus.Shared.Extensions;

namespace ECampus.FrontEnd.Requests;

public class UserRelationshipsRequests : IUserRelationshipsRequests
{
    private readonly IHttpClientFactory _client;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserRelationshipsRequests(IHttpClientFactory client, IHttpContextAccessor httpContextAccessor)
    {
        _client = client;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task SaveAuditory(int auditoryId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/Users/auditory?userId={UserId()}&auditoryId={auditoryId}");
        var response = await _client.CreateClient(RequestOptions.ClientName).SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveSavedAuditory(int auditoryId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/Users/auditory?userId={UserId()}&auditoryId={auditoryId}");
        var response = await _client.CreateClient(RequestOptions.ClientName).SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task SaveGroup(int groupId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/Users/group?userId={UserId()}&groupId={groupId}");
        var response = await _client.CreateClient(RequestOptions.ClientName).SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveSavedGroup(int groupId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/Users/group?userId={UserId()}&groupId={groupId}");
        var response = await _client.CreateClient(RequestOptions.ClientName).SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task SaveTeacher(int teacherId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/Users/teacher?userId={UserId()}&teacherId={teacherId}");
        var response = await _client.CreateClient(RequestOptions.ClientName).SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveSavedTeacher(int teacherId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/Users/teacher?userId={UserId()}&teacherId={teacherId}");
        var response = await _client.CreateClient(RequestOptions.ClientName).SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    private int UserId()
    {
        return _httpContextAccessor.HttpContext?.User.GetId() ?? throw new UnauthorizedAccessException();
    }
}