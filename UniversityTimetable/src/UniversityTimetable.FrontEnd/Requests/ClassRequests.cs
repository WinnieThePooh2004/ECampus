using System.Diagnostics;
using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Requests;

public class ClassRequests : IClassRequests
{
    private readonly IHttpClientFactory _client;
    private readonly JsonSerializerOptions _options;
    public ClassRequests(IHttpClientFactory clientFactory, JsonSerializerOptions options)
    {
        _options = options;
        _client = clientFactory;
    }

    public async Task<Timetable> AuditoryTimetable(int auditoryId)
    {
        var response = await _client.CreateClient("UTApi").GetAsync($"/api/Timetable/Auditory/{auditoryId}");
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<Timetable>(await response.Content.ReadAsStringAsync(), _options)
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }

    public async Task<Timetable> GroupTimetable(int groupId)
    {
        var response = await _client.CreateClient("UTApi").GetAsync($"/api/Timetable/Group/{groupId}");
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<Timetable>(await response.Content.ReadAsStringAsync(), _options)
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }

    public async Task<Timetable> TeacherTimetable(int teacherId)
    {
        var response = await _client.CreateClient("UTApi").GetAsync($"/api/Timetable/Teacher/{teacherId}");
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<Timetable>(await response.Content.ReadAsStringAsync(), _options)
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }
}