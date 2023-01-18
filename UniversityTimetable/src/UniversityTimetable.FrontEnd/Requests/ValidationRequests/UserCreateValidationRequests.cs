using System.Diagnostics;
using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces.Validation;
using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.FrontEnd.Requests.ValidationRequests;

public class UserCreateValidationRequests : ICreateValidationRequests<UserDto>
{
    private readonly IHttpClientFactory _client;
    private readonly JsonSerializerOptions _serializerOptions;

    public UserCreateValidationRequests(IHttpClientFactory client, JsonSerializerOptions serializerOptions)
    {
        _client = client;
        _serializerOptions = serializerOptions;
    }

    public async Task<ValidationResult> ValidateAsync(UserDto data)
    {
        var response = await _client.CreateClient("UTApi").PutAsJsonAsync("api/Users/Validate/Create", data);
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<ValidationResult>(await response.Content.ReadAsStringAsync(), _serializerOptions)
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }
}