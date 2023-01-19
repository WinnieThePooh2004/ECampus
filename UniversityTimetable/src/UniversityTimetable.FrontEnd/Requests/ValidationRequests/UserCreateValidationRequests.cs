using System.Diagnostics;
using Newtonsoft.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces.Validation;
using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.FrontEnd.Requests.ValidationRequests;

public class UserCreateValidationRequests : ICreateValidationRequests<UserDto>
{
    private readonly IHttpClientFactory _client;

    public UserCreateValidationRequests(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<ValidationResult> ValidateAsync(UserDto data)
    {
        var response = await _client.CreateClient("UTApi").PutAsJsonAsync("api/Users/Validate/Create", data);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<ValidationResult>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }
}