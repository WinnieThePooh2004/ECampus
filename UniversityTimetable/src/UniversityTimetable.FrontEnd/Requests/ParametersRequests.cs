using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.Extensions;

namespace UniversityTimetable.FrontEnd.Requests
{
    public class ParametersRequests<TData, TParameters> : IParametersRequests<TData, TParameters>
        where TData : class
        where TParameters : IQueryParameters
    {
        private readonly string _controllerName;
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public ParametersRequests(IHttpClientFactory clientFactory, JsonSerializerOptions options)
        {
            _options = options;
            _client = clientFactory.CreateClient("UTApi");
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
