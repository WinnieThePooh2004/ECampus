using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Requests;

public class PasswordChangeRequests : IPasswordChangeRequests
{
    private readonly IHttpClientFactory _client;

    public PasswordChangeRequests(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task ChangePassword(PasswordChangeDto passwordChange)
    {
        var response = await _client.CreateClient("UTApi").PutAsJsonAsync("api/Users/changePassword", passwordChange);
        response.EnsureSuccessStatusCode();
    }
}