using Microsoft.Extensions.Options;
using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.FrontEnd.Requests.Options;
using UniversityTimetable.Shared.Extensions;

namespace UniversityTimetable.FrontEnd.Requests;

public class BaseRequests<TData> : IBaseRequests<TData>
{
    private readonly string _controllerName;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;
    public BaseRequests(IHttpClientFactory clientFactory, JsonSerializerOptions jsonOptions, IRequestOptions options)
    {
        _jsonOptions = jsonOptions;
        _jsonOptions = jsonOptions;
        _client = clientFactory.CreateClient("UTApi");
        _controllerName = options.GetControllerName(typeof(TData));
    }
    public async Task<TData> CreateAsync(TData entity)
    {
        var response = await _client.PostAsJsonAsync($"/api/{_controllerName}", entity);
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<TData>(await response.Content.ReadAsStringAsync(), _jsonOptions);
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
        return JsonSerializer.Deserialize<TData>(await response.Content.ReadAsStringAsync(), _jsonOptions);
    }

    public async Task<TData> UpdateAsync(TData entity)
    {
        var response = await _client.PutAsJsonAsync($"/api/{_controllerName}", entity);
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<TData>(await response.Content.ReadAsStringAsync(), _jsonOptions);
    }
}