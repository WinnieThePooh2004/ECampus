using System.Diagnostics;
using Newtonsoft.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.FrontEnd.Requests.Options;
using UniversityTimetable.Shared.Extensions;

namespace UniversityTimetable.FrontEnd.Requests;

public class ParametersRequests<TData, TParameters> : IParametersRequests<TData, TParameters>
    where TData : class
    where TParameters : IQueryParameters
{
    private readonly string _controllerName;
    private readonly HttpClient _client;

    public ParametersRequests(IHttpClientFactory clientFactory, IRequestOptions options)
    {
        _client = clientFactory.CreateClient("UTApi");
        _controllerName = options.GetControllerName(typeof(TData));
    }

    public async Task<ListWithPaginationData<TData>> GetByParametersAsync(TParameters parameters)
    {
        var response = await _client.GetAsync($"/api/{_controllerName}?{parameters.ToQueryString()}");
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<ListWithPaginationData<TData>>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot get list with objects of type {typeof(TData)}");
    }
}