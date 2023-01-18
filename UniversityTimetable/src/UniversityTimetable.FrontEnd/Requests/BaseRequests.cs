using System.Diagnostics;
using Newtonsoft.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.FrontEnd.Requests.Options;

namespace UniversityTimetable.FrontEnd.Requests;

public class BaseRequests<TData> : IBaseRequests<TData>
{
    private readonly string _controllerName;
    private readonly IHttpClientFactory _client;

    public BaseRequests(IHttpClientFactory client, IRequestOptions options)
    {
        _client = client;
        _controllerName = options.GetControllerName(typeof(TData));
    }

    public async Task<TData> CreateAsync(TData entity)
    {
        var response = await _client.CreateClient("UTApi").PostAsJsonAsync($"/api/{_controllerName}", entity);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<TData>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(TData)}");
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _client.CreateClient("UTApi").DeleteAsync($"/api/{_controllerName}/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<TData> GetByIdAsync(int id)
    {
        var response = await _client.CreateClient("UTApi").GetAsync($"/api/{_controllerName}/{id}");
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<TData>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(TData)}");
    }

    public async Task<TData> UpdateAsync(TData entity)
    {
        var response = await _client.CreateClient("UTApi").PutAsJsonAsync($"/api/{_controllerName}", entity);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<TData>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(TData)}");
    }
}