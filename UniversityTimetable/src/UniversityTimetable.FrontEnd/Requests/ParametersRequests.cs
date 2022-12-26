using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.FrontEnd.Requests.Options;
using UniversityTimetable.Shared.Extensions;

namespace UniversityTimetable.FrontEnd.Requests
{
    public class ParametersRequests<TData, TParameters> : IParametersRequests<TData, TParameters>
        where TData : class
        where TParameters : IQueryParameters
    {
        private readonly string _controllerName;
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonOptions;

        public ParametersRequests(IHttpClientFactory clientFactory, JsonSerializerOptions jsonOptions, IRequestOptions options)
        {
            _jsonOptions = jsonOptions;
            _client = clientFactory.CreateClient("UTApi");
            _controllerName = options.GetControllerName(typeof(TData));
        }

        public async Task<ListWithPaginationData<TData>> GetByParametersAsync(TParameters parameters)
        {
            var response = await _client.GetAsync($"/api/{_controllerName}?{parameters.ToQueryString()}");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<ListWithPaginationData<TData>>(await response.Content.ReadAsStringAsync(), _jsonOptions);
        }
    }
}
