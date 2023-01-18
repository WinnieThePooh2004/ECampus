using System.Diagnostics;
using Newtonsoft.Json;
using UniversityTimetable.FrontEnd.Requests.Interfaces.Validation;
using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.FrontEnd.Requests.ValidationRequests;

public class ClassValidationRequests : IValidationRequests<ClassDto>
{
    private readonly IHttpClientFactory _client;

    public ClassValidationRequests(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<ValidationResult> ValidateAsync(ClassDto model)
    {
        var response = await _client.CreateClient("UTApi").PutAsJsonAsync("/api/Timetable/Validate", model);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<ValidationResult>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }
}