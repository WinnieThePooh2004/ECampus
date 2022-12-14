using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.Extentions;

namespace UniversityTimetable.FrontEnd.Requests
{
    public class Requests<TData, TParameters> : BaseRequests<TData>, IRequests<TData, TParameters>
        where TData : class
        where TParameters : IQueryParameters
    {
        public Requests(IHttpClientFactory clientFactory) : base(clientFactory)
        {

        }

        public async Task<ListWithPaginationData<TData>> GetByParametersAsync(TParameters parameters)
        {
            var response = await _client.GetAsync($"/api/{_controllerName}?{parameters.ToQueryString()}");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<ListWithPaginationData<TData>>(await response.Content.ReadAsStringAsync(), _options);
        }
    }
}
