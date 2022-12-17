using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.Extentions;

namespace UniversityTimetable.FrontEnd.Requests
{
    public class ParametersRequests<TData, TParameters> : IParametersIRequests<TData, TParameters>
        where TData : class
        where TParameters : IQueryParameters
    {
        private readonly string _controllerName;
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public ParametersRequests(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("UTApi");
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _controllerName = Options.Requests.ControllerNames[typeof(TData)];
        }

        public async Task<ListWithPaginationData<TData>> GetByParametersAsync(TParameters parameters)
        {
            var response = await _client.GetAsync($"/api/{_controllerName}?{parameters.ToQueryString()}");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<ListWithPaginationData<TData>>(await response.Content.ReadAsStringAsync(), _options);
        }
    }
}
