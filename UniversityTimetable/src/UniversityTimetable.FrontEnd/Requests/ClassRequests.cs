using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Requests
{
    public class ClassRequests : IClassRequests
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        public ClassRequests(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("UTApi");
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, IncludeFields = true };
        }

        public async Task<Timetable> AuditoryTimetable(int groupId)
        {
            var response = await _client.GetAsync($"/api/Timetable/Auditory/{groupId}");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<Timetable>(await response.Content.ReadAsStringAsync(), _options);
        }

        public async Task<ClassDTO> CreateAsync(ClassDTO entity)
        {
            var response = await _client.PostAsJsonAsync($"/api/Timetable", entity);
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<ClassDTO>(await response.Content.ReadAsStringAsync(), _options);
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _client.DeleteAsync($"/api/Timetable/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<ClassDTO> GetByIdAsync(int id)
        {
            var response = await _client.GetAsync($"/api/Timetable/{id}");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<ClassDTO>(await response.Content.ReadAsStringAsync(), _options);
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

        public async Task<ClassDTO> UpdateAsync(ClassDTO entity)
        {
            var response = await _client.PutAsJsonAsync($"/api/Timetable", entity);
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<ClassDTO>(await response.Content.ReadAsStringAsync(), _options);
        }
    }
}
