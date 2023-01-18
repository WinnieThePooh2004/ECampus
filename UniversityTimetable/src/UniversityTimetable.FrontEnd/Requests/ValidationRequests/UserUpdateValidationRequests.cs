using System.Diagnostics;
using Newtonsoft.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces.Validation;
using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.FrontEnd.Requests.ValidationRequests;

public class UserUpdateValidationRequests : IUpdateValidationRequests<UserDto>
{
    private readonly IHttpClientFactory _client;

    public UserUpdateValidationRequests(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<ValidationResult> ValidateAsync(UserDto data)
    {
        var response = await _client.CreateClient("UTApi").PutAsJsonAsync("api/Users/Validate/Update", data);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<ValidationResult>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }
}