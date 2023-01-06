using System.Diagnostics;
using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Requests;

public class ClassRequests : IClassRequests
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _options;
    public ClassRequests(IHttpClientFactory clientFactory, JsonSerializerOptions options)
    {
        _options = options;
        _client = clientFactory.CreateClient("UTApi");
    }

    public async Task<Timetable> AuditoryTimetable(int groupId)
    {
        var response = await _client.GetAsync($"/api/Timetable/Auditory/{groupId}");
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<Timetable>(await response.Content.ReadAsStringAsync(), _options)
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }

    public async Task<Timetable> GroupTimetable(int groupId)
    {
        var response = await _client.GetAsync($"/api/Timetable/Group/{groupId}");
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<Timetable>(await response.Content.ReadAsStringAsync(), _options)
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }

    public async Task<Timetable> TeacherTimetable(int groupId)
    {
        var response = await _client.GetAsync($"/api/Timetable/Teacher/{groupId}");
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<Timetable>(await response.Content.ReadAsStringAsync(), _options)
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }
}