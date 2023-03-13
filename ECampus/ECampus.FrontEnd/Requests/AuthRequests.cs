using System.Diagnostics;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.FrontEnd.Requests.Interfaces.Validation;
using ECampus.FrontEnd.Requests.Options;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Responses.Auth;
using ECampus.Domain.Validation;
using Newtonsoft.Json;

namespace ECampus.FrontEnd.Requests;

public class AuthRequests : IAuthRequests, IValidationRequests<LoginDto>, IValidationRequests<RegistrationDto>
{
    private readonly IHttpClientFactory _client;

    public AuthRequests(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<LoginResponse> LoginAsync(LoginDto login)
    {
        var response = await _client.CreateClient(RequestOptions.ClientName).PostAsJsonAsync("api/Auth/login", login);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }

    public async Task<LoginResponse> SignUpAsync(RegistrationDto registrationDto)
    {
        var response = await _client.CreateClient(RequestOptions.ClientName)
            .PostAsJsonAsync("api/Auth/signup", registrationDto);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }

    public async Task<ValidationResult> ValidateAsync(LoginDto data)
    {
        var response = await _client.CreateClient(RequestOptions.ClientName)
            .PutAsJsonAsync("api/Auth/signup/validate", data);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<ValidationResult>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }

    public async Task<ValidationResult> ValidateAsync(RegistrationDto data)
    {
        var response = await _client.CreateClient(RequestOptions.ClientName)
            .PutAsJsonAsync("api/Auth/signup/validate", data);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<ValidationResult>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot deserialize object of type {typeof(UserDto)}");
    }
}