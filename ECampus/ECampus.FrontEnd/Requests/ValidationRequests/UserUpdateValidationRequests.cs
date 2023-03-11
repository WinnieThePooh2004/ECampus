using System.Diagnostics;
using ECampus.FrontEnd.Requests.Interfaces.Validation;
using ECampus.FrontEnd.Requests.Options;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Validation;
using Newtonsoft.Json;

namespace ECampus.FrontEnd.Requests.ValidationRequests;

public class UserUpdateValidationRequests : IUpdateValidationRequests<UserDto>
{
    private readonly IHttpClientFactory _client;

    public UserUpdateValidationRequests(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<ValidationResult> ValidateAsync(UserDto data)
    {
        var response = await _client.CreateClient(RequestOptions.ClientName).PutAsJsonAsync("api/Users/Validate/Update", data);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<ValidationResult>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }
}