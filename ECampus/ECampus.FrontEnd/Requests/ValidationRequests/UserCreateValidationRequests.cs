using System.Diagnostics;
using ECampus.FrontEnd.Requests.Interfaces.Validation;
using ECampus.FrontEnd.Requests.Options;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Validation;
using Newtonsoft.Json;

namespace ECampus.FrontEnd.Requests.ValidationRequests;

public class UserCreateValidationRequests : ICreateValidationRequests<UserDto>
{
    private readonly IHttpClientFactory _client;

    public UserCreateValidationRequests(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<ValidationResult> ValidateAsync(UserDto data)
    {
        var response = await _client.CreateClient(RequestOptions.ClientName).PutAsJsonAsync("api/Users/Validate/Create", data);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<ValidationResult>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }
}