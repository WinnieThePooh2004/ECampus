using Microsoft.Extensions.Options;
using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.Extentions;

namespace UniversityTimetable.FrontEnd.Requests
{
    public class BaseRequests<TData> : IBaseRequests<TData>
    {
        protected readonly string _controllerName;
        protected readonly HttpClient _client;
        protected readonly JsonSerializerOptions _options;
        public BaseRequests(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("UTApi");
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _controllerName = Options.Requests.ControllerNames[typeof(TData)];
        }
        public async Task<TData> CreateAsync(TData entity)
        {
            var response = await _client.PostAsJsonAsync($"/api/{_controllerName}", entity);
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<TData>(await response.Content.ReadAsStringAsync(), _options);
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _client.DeleteAsync($"/api/{_controllerName}/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<TData> GetByIdAsync(int id)
        {
            var response = await _client.GetAsync($"/api/{_controllerName}/{id}");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<TData>(await response.Content.ReadAsStringAsync(), _options);
        }

        public async Task<TData> UpdateAsync(TData entity)
        {
            var response = await _client.PutAsJsonAsync($"/api/{_controllerName}", entity);
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<TData>(await response.Content.ReadAsStringAsync(), _options);
        }
    }
}
