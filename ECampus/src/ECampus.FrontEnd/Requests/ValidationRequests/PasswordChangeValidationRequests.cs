using System.Diagnostics;
using ECampus.FrontEnd.Requests.Interfaces.Validation;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;
using Newtonsoft.Json;

namespace ECampus.FrontEnd.Requests.ValidationRequests;

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