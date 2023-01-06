using System.Diagnostics;
using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces.Validation;

namespace UniversityTimetable.FrontEnd.Requests.ValidationRequests;

public class UserUpdateValidationRequests : IUpdateValidationRequests<UserDto>
{
    private readonly IHttpClientFactory _client;
    private readonly JsonSerializerOptions _serializerOptions;

    public UserUpdateValidationRequests(IHttpClientFactory client, JsonSerializerOptions serializerOptions)
    {
        _client = client;
        _serializerOptions = serializerOptions;
    }

    public async Task<List<KeyValuePair<string, string>>> ValidateAsync(UserDto data)
    {
        var response = await _client.CreateClient("UTApi").PutAsJsonAsync("api/Users/Validate/Update", data);
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(await response.Content.ReadAsStringAsync(), _serializerOptions)
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }
}