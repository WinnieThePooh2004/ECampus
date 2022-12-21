using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Requests
{
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
            return JsonSerializer.Deserialize<Timetable>(await response.Content.ReadAsStringAsync(), _options);
        }

        public async Task<Timetable> GroupTimetable(int groupId)
        {
            var response = await _client.GetAsync($"/api/Timetable/Group/{groupId}");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<Timetable>(await response.Content.ReadAsStringAsync(), _options);
        }

        public async Task<Timetable> TeacherTimetable(int groupId)
        {
            var response = await _client.GetAsync($"/api/Timetable/Teacher/{groupId}");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<Timetable>(await response.Content.ReadAsStringAsync(), _options);
        }

        public async Task<Dictionary<string, string>> ValidateAsync(ClassDto model)
        {
            var response = await _client.PutAsJsonAsync("/api/Timetable/Validate", model);
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
        }
    }
}
