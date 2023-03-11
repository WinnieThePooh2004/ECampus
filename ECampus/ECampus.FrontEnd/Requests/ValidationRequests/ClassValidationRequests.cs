using System.Diagnostics;
using ECampus.FrontEnd.Requests.Interfaces.Validation;
using ECampus.FrontEnd.Requests.Options;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Validation;
using Newtonsoft.Json;

namespace ECampus.FrontEnd.Requests.ValidationRequests;

public class ClassValidationRequests : IValidationRequests<ClassDto>
{
    private readonly IHttpClientFactory _client;

    public ClassValidationRequests(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<ValidationResult> ValidateAsync(ClassDto model)
    {
        var response = await _client.CreateClient(RequestOptions.ClientName)
            .PutAsJsonAsync("/api/Timetable/Validate", model);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<ValidationResult>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }
}