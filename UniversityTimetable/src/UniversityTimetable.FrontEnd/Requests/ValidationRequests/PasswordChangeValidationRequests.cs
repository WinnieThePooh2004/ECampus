using System.Diagnostics;
using Newtonsoft.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces.Validation;
using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.FrontEnd.Requests.ValidationRequests;

public class PasswordChangeValidationRequests : IValidationRequests<PasswordChangeDto>
{
    private readonly IHttpClientFactory _client;

    public PasswordChangeValidationRequests(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<ValidationResult> ValidateAsync(PasswordChangeDto model)
    {
        var response = await _client.CreateClient("UTApi").PutAsJsonAsync("api/Users/changePassword/validate", model);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<ValidationResult>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }
}