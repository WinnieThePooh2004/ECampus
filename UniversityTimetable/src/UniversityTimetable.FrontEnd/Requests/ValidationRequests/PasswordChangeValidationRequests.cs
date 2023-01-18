using System.Diagnostics;
using System.Text.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces.Validation;
using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.FrontEnd.Requests.ValidationRequests;

public class PasswordChangeValidationRequests : IValidationRequests<PasswordChangeDto>
{
    private readonly IHttpClientFactory _client;
    private readonly JsonSerializerOptions _options;

    public PasswordChangeValidationRequests(IHttpClientFactory client, JsonSerializerOptions options)
    {
        _client = client;
        _options = options;
    }

    public async Task<ValidationResult> ValidateAsync(PasswordChangeDto model)
    {
        var response = await _client.CreateClient("UTApi").PutAsJsonAsync("api/Users/changePassword/validate", model);
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<ValidationResult>(
                   await response.Content.ReadAsStreamAsync(), _options)
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }
}