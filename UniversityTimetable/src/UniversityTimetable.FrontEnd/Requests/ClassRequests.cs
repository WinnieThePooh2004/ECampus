using System.Diagnostics;
using Newtonsoft.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Requests;

public class ClassRequests : IClassRequests
{
    private readonly IHttpClientFactory _client;

    public ClassRequests(IHttpClientFactory clientFactory)
    {
        _client = clientFactory;
    }

    public async Task<Timetable> AuditoryTimetable(int auditoryId)
    {
        var response = await _client.CreateClient("UTApi").GetAsync($"/api/Timetable/Auditory/{auditoryId}");
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<Timetable>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }

    public async Task<Timetable> GroupTimetable(int groupId)
    {
        var response = await _client.CreateClient("UTApi").GetAsync($"/api/Timetable/Group/{groupId}");
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<Timetable>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }

    public async Task<Timetable> TeacherTimetable(int teacherId)
    {
        var response = await _client.CreateClient("UTApi").GetAsync($"/api/Timetable/Teacher/{teacherId}");
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<Timetable>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }
}